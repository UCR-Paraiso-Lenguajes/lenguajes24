using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
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
                        use store;
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
                        use store;
                        INSERT INTO saleLines (productId, purchaseNumber, price, quantity)
                        VALUES (@product_Id, @purchase_Number, @product_Price, @product_Quantity);";

                        foreach (var product in sale.Products)
                        {
                            using (var insertCommandLines = new MySqlCommand(insertQueryLines, connection, transaction))
                            {
                                insertCommandLines.Parameters.AddWithValue("@product_Id", product.Id);
                                insertCommandLines.Parameters.AddWithValue("@purchase_Number", sale.PurchaseNumber);
                                insertCommandLines.Parameters.AddWithValue("@product_Price", product.Price);
                                insertCommandLines.Parameters.AddWithValue("@product_Quantity", product.Quantity);
                                await insertCommandLines.ExecuteNonQueryAsync();
                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public SalesReport GetSalesReport(DateTime date)
        {
            if (date == DateTime.MinValue)
                throw new ArgumentException("Date cannot be:", nameof(date));

            Task<List<WeeklyReport>> weeklySalesTask = GetWeeklySalesAsync(date);
            Task<List<DailyReport>> dailySalesTask = GetDailySalesAsync(date);
            Task.WaitAll(weeklySalesTask, dailySalesTask);
            List<WeeklyReport> weeklySales = weeklySalesTask.Result;
            List<DailyReport> dailySales = dailySalesTask.Result;

            SalesReport salesReport = new SalesReport(dailySales, weeklySales);
            return salesReport;
        }

        public async Task<List<WeeklyReport>> GetWeeklySalesAsync(DateTime date)
        {
            List<WeeklyReport> weeklySales = new List<WeeklyReport>();

            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"
                    use store;
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

        public async Task<List<DailyReport>> GetDailySalesAsync(DateTime date)
        {
            List<DailyReport> dailySales = new List<DailyReport>();

            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"
                    use store;
                    SELECT purchase_date, purchase_number, total
                    FROM sales
                    WHERE DATE(purchase_date) = DATE(@date);";

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
                            DailyReport dailyReport = new DailyReport(purchaseDate, purchaseNumber, total);
                            dailySales.Add(dailyReport);
                        }
                    }
                }
            }
            return dailySales;
        }
    }
}