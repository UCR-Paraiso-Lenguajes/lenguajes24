using System;
using Core;
using MySqlConnector;
using StoreAPI.models;

namespace StoreAPI.Database
{
    public sealed class SaleBD
    {
        public async Task SaveAsync(Sale sale)
        {
            if (sale == null) throw new ArgumentNullException($"The {nameof(sale)} object cannot be null.");
            if (sale.Products == null) throw new ArgumentException($"The {nameof(sale)} must contain at least one product.");


            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertQuery = @"
                            INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                            VALUES (@date, @total, @PaymentMethod, @orderNumber);";

                        using (var command = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@date", DateTime.Now);
                            command.Parameters.AddWithValue("@total", sale.Amount);
                            command.Parameters.AddWithValue("@PaymentMethod", (int)sale.PaymentMethod);
                            command.Parameters.AddWithValue("@orderNumber", sale.NumberOrder);

                            await command.ExecuteNonQueryAsync();
                            long saleId = command.LastInsertedId;

                            await InsertSaleLinesAsync(saleId, sale.Products.ToList(), connection, transaction);
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error saving the sale in the database.");
                    }
                }
            }
        }
        private async Task InsertSaleLinesAsync(long saleId, List<Product> products, MySqlConnection connection, MySqlTransaction transaction)
        {
            if (products == null || products.Count == 0) throw new ArgumentException("The products list cannot be null or empty.", nameof(products));
            try
            {
                foreach (var product in products)
                {
                    string insertSaleLineQuery = @"
                        INSERT INTO saleLines (sale_id, product_id, quantity, final_price)
                        VALUES (@saleId, @productId, @quantity, @finalPrice);";

                    using (var insertCommand = new MySqlCommand(insertSaleLineQuery, connection, transaction))
                    {
                        insertCommand.Parameters.AddWithValue("@saleId", saleId);
                        insertCommand.Parameters.AddWithValue("@productId", product.Id);
                        insertCommand.Parameters.AddWithValue("@quantity", product.Quantity);
                        insertCommand.Parameters.AddWithValue("@finalPrice", product.Price);
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception)
            {
                // Si ocurre un error, deshace la transacción
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while saving the sale. Please check the logs for more details.");
            }
        }

        public async Task<IEnumerable<DaySalesReports>> GetDailySalesAsync(DateTime date)
        {
            if (date == DateTime.MinValue) throw new ArgumentException($"Invalid date provided: {nameof(date)}");

            List<DaySalesReports> dailySales = new List<DaySalesReports>();

            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"
            use store;
            SELECT S.purchase_number AS purchase_Number, S.purchase_date AS purchase_date, S.total AS total,  SUM(Sl.quantity) AS quantity, GROUP_CONCAT(P.name SEPARATOR ', ') AS productsName
            FROM sales S
            INNER JOIN saleLines Sl ON S.id = Sl.sale_id
			INNER JOIN products P ON Sl.product_id = P.id 
            WHERE DATE(S.purchase_date) = @date
            GROUP BY S.purchase_number, S.purchase_date, S.total";

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@date", date);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime purchaseDate = reader.GetDateTime("purchase_date");
                            string purchaseNumber = reader.GetString("purchase_Number");
                            int quantity = reader.GetInt32("quantity");
                            decimal total = reader.GetDecimal("total");
                            string productsString = reader.GetString("productsName");
                            DaySalesReports dayReport = new DaySalesReports(purchaseDate, purchaseNumber.ToString(), quantity, total, productsString);
                            dailySales.Add(dayReport);
                        }
                    }
                }
            }

            return dailySales;
        }

        public async Task<IEnumerable<WeekSalesReport>> GetWeeklySalesAsync(DateTime date)
        {
            if (date == DateTime.MinValue) throw new ArgumentException($"Invalid date provided: {nameof(date)}");

            List<WeekSalesReport> weeklySales = new List<WeekSalesReport>();

            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"
                use store;
                SELECT DAYNAME(S.purchase_date) AS day, SUM(S.total) AS total
                FROM sales S 
                WHERE YEARWEEK(S.purchase_date) = YEARWEEK(@date)
                GROUP BY DAYNAME(S.purchase_date);";

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@date", date);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string? day = reader.GetString("day").ToString();
                            decimal total = reader.GetDecimal("total");
                            WeekSalesReport weekSalesReport = new WeekSalesReport(day, total);
                            weeklySales.Add(weekSalesReport);
                        }
                    }
                }
            }

            return weeklySales;
        }

    }
}