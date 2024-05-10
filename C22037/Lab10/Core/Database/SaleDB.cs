using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using TodoApi.Models;

namespace TodoApi.Database
{
    public sealed class SaleDB
    {
        public async Task SaveAsync(Sale sale)
        {
            if (sale == null) throw new ArgumentException($"{nameof(sale)} cannot be null.");
            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertQuery = @"
                            INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                            VALUES (@purchase_date, @total, @payment_method, @purchase_number);";

                        using (var insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                            insertCommand.Parameters.AddWithValue("@total", sale.Amount);
                            insertCommand.Parameters.AddWithValue("@payment_method", sale.PaymentMethod);
                            insertCommand.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        string insertQueryLines = @"
                            INSERT INTO saleLines (productId, purchaseNumber, price)
                            VALUES (@product_Id, @purchase_Number, @product_Price);";

                        foreach (var product in sale.Products)
                        {
                            using (var insertCommandLines = new MySqlCommand(insertQueryLines, connection, transaction))
                            {
                                insertCommandLines.Parameters.AddWithValue("@product_Id", product.Id);
                                insertCommandLines.Parameters.AddWithValue("@purchase_Number", sale.PurchaseNumber);
                                insertCommandLines.Parameters.AddWithValue("@product_Price", product.Price);
                                await insertCommandLines.ExecuteNonQueryAsync();
                            }
                        }

                        // Commit the transaction if all inserts are successful
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction if an error occurs
                        await transaction.RollbackAsync();
                    }
                }
            }
        }

        public async Task<SalesReport> GetSalesReportAsync(DateTime date)
        {
            if (date == DateTime.MinValue) throw new ArgumentException("Date cannot be:", nameof(date));
            List<WeeklyReport> weeklySales = (List<WeeklyReport>)await GetWeeklySalesAsync(date);
            List<DailyReport> dailySales = (List<DailyReport>)await GetDailySalesAsync(date);
            SalesReport salesReport = new SalesReport(dailySales, weeklySales);
            return salesReport;
        }

        private async Task<IEnumerable<WeeklyReport>> GetWeeklySalesAsync(DateTime date)
        {
            List<WeeklyReport> weeklySales = new List<WeeklyReport>();

            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"
                    SELECT DAYNAME(sale.purchase_date) AS day, SUM(sale.total) AS total
                    FROM sales sale 
                    WHERE YEARWEEK(sale.purchase_date) = YEARWEEK(@date)
                    GROUP BY DAYNAME(sale.purchase_date);";

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@date", date);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string day = reader.GetString("day");
                            decimal total = reader.GetDecimal("total");
                            WeeklyReport weeklyReport = new WeeklyReport(day, total);
                            weeklySales.Add(weeklyReport);
                        }
                    }
                }
            }
            return weeklySales;
        }

        private async Task<IEnumerable<DailyReport>> GetDailySalesAsync(DateTime date)
        {
            List<DailyReport> dailylySales = new List<DailyReport>();

            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"
                    SELECT purchase_date, purchase_number, total
                    FROM sales
                    WHERE YEARWEEK(purchase_date) = YEARWEEK(@date)";

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@date", date);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime purchaseDate = reader.GetDateTime("purchase_date");
                            string purchaseNumber = reader.GetString("purchase_number");
                            decimal total = reader.GetDecimal("total");
                            DailyReport dailyReport = new DailyReport(purchaseDate, purchaseNumber.ToString(), total); // Convertir purchaseNumber a string si es necesario
                            dailylySales.Add(dailyReport);
                        }
                    }
                }
            }
            return dailylySales;
        }
    }
}