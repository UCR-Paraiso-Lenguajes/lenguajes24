using System;
using MySqlConnector;
using storeApi.Models;

namespace storeApi.Database
{
    public sealed class SaleDB
    {
        public async Task SaveAsync(Sale sale)
        {
            if (sale == null) throw new ArgumentException("Sale must contain at least one product.");

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=mysql;Uid=root;Pwd=123456;"))
            {
                await connection.OpenAsync();

                using (MySqlTransaction transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertQuery = @"
                            use store;

                            INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                            VALUES (@purchase_date, @total, @payment_method, @purchase_number);"
                            ;

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                            command.Parameters.AddWithValue("@total", sale.Amount);
                            command.Parameters.AddWithValue("@payment_method", sale.PaymentMethod);
                            command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                            await command.ExecuteNonQueryAsync();
                        }

                        string insertQueryLineDB = @"
                            use store;
                            INSERT INTO saleLines (productId, purchaseNumber, price)
                            VALUES (@product_Id, @purchase_Number, @product_Price)";
                        foreach (var product in sale.Products)
                        {
                            using (var insertCommand = new MySqlCommand(insertQueryLineDB, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@product_Id", product.Id);
                                insertCommand.Parameters.AddWithValue("@purchase_Number", sale.PurchaseNumber);
                                insertCommand.Parameters.AddWithValue("@product_Price", product.Price);
                                await insertCommand.ExecuteNonQueryAsync();
                            }
                        }

                        await transaction.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        throw new ArgumentException("An error occurred while saving sale to database:" + ex.Message);
                    }
                }
            }
        }
        public async Task<Dictionary<string, decimal>> getWeekSalesAsync(DateTime date)
        {
            Dictionary<string, decimal> weekSales = new Dictionary<string, decimal>();

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=mysql;Uid=root;Pwd=123456;"))
            {
                await connection.OpenAsync();

                string selectQuery = @"
                    USE store;

                    SELECT DAYNAME(sale.purchase_date) AS day,
                           SUM(sale.total) AS total
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

                            if (weekSales.ContainsKey(day))
                            {
                                weekSales[day] += total;
                            }
                            else
                            {
                                weekSales.Add(day, total);
                            }
                        }
                    }
                }
            }

            return weekSales;
        }


        public async Task<List<(string purchaseNumber, decimal total)>> getDailySales(DateTime date)
        {
            List<(string purchaseNumber, decimal total)> dailySales = new List<(string purchaseNumber, decimal total)>();

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=mysql;Uid=root;Pwd=123456;"))
            {
                await connection.OpenAsync(); // Abrir la conexión de forma asíncrona

                string selectQuery = @"
            USE store;

            SELECT sale.purchase_number,
                   sale.total
            FROM sales sale
            WHERE DATE(sale.purchase_date) = DATE(@date);";

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@date", date);

                    using (var reader = await command.ExecuteReaderAsync()) // Ejecutar la consulta de forma asíncrona
                    {
                        while (await reader.ReadAsync()) // Leer los resultados de forma asíncrona
                        {
                            string purchaseNumber = reader.GetString("purchase_number");
                            decimal total = reader.GetDecimal("total");
                            dailySales.Add((purchaseNumber, total));
                        }
                    }
                }
            }

            return dailySales;
        }







    }
}