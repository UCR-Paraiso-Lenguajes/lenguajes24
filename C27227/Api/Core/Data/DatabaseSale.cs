using Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEStoreApi
{
    public sealed class DatabaseSale
    {
        public async Task SaveAsync(Sale sale)
        {
            if (sale == null) { throw new ArgumentNullException($"El objeto {nameof(Sale)} no puede ser nulo."); }
            bool areSaleProductsInvalid = sale.Products == null || !sale.Products.Any();
            if (areSaleProductsInvalid)
            {
                throw new ArgumentException($"La {nameof(sale.Products)} en la venta no puede ser nula o vacía.");
            }
            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlTransaction transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // Insert address
                        string insertAddressQuery = @"
                        INSERT INTO addresses (street, city, state, zipCode, country)
                        VALUES (@street, @city, @state, @zipCode, @country);
                        SELECT LAST_INSERT_ID();";

                        int addressId;
                        using (MySqlCommand addressCommand = new MySqlCommand(insertAddressQuery, connection, transaction))
                        {
                            addressCommand.Parameters.AddWithValue("@street", sale.Address.Street);
                            addressCommand.Parameters.AddWithValue("@city", sale.Address.City);
                            addressCommand.Parameters.AddWithValue("@state", sale.Address.State);
                            addressCommand.Parameters.AddWithValue("@zipCode", sale.Address.ZipCode);
                            addressCommand.Parameters.AddWithValue("@country", sale.Address.Country);

                            addressId = Convert.ToInt32(await addressCommand.ExecuteScalarAsync());
                        }

                        // Insert sale with address_id
                        string insertSaleQuery = @"
                        INSERT INTO Sales (purchase_date, total, payment_method, purchaseNumber, address_id)
                        VALUES (@purchase_date, @total, @payment_method, @purchase_number, @address_id);";

                        using (MySqlCommand command = new MySqlCommand(insertSaleQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                            command.Parameters.AddWithValue("@total", sale.Total);
                            command.Parameters.AddWithValue("@payment_method", (int)sale.PaymentMethod);
                            command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                            command.Parameters.AddWithValue("@address_id", addressId);
                            await command.ExecuteNonQueryAsync();

                            foreach (var product in sale.Products)
                            {
                                string insertLineQuery = @"
                                INSERT INTO Lines_Sales (id_Sale, id_Product, quantity, price)
                                VALUES (@id_Sale, @id_Product, @quantity, @price);";

                                using (MySqlCommand lineCommand = new MySqlCommand(insertLineQuery, connection, transaction))
                                {
                                    lineCommand.Parameters.AddWithValue("@id_Sale", sale.PurchaseNumber);
                                    lineCommand.Parameters.AddWithValue("@id_Product", product.Id);
                                    lineCommand.Parameters.AddWithValue("@quantity", product.Quantity);
                                    lineCommand.Parameters.AddWithValue("@price", product.Price);
                                    await lineCommand.ExecuteNonQueryAsync();
                                }
                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error al guardar la venta en la base de datos", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<SaleDetails>> GetDailySalesReportAsync(DateTime date)
        {
            if (date == DateTime.MinValue) { throw new ArgumentException("La fecha no puede ser mayor .", nameof(date)); }

            List<SaleDetails> salesReport = new List<SaleDetails>();

            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

            string query = @"
            SELECT s.purchaseNumber, s.total, s.purchase_date, ls.quantity, p.name
            FROM Sales s
            JOIN Lines_Sales ls ON s.purchaseNumber = ls.id_Sale
            JOIN paymentMethod pm ON s.payment_method = pm.id
            JOIN products p ON p.id = ls.id_Product
            WHERE s.purchase_date >= @date
                AND s.purchase_date < DATE_ADD(@date, INTERVAL 1 DAY);
            ";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", date.Date);

                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string purchaseNumber = reader.GetString(0);
                            decimal total = reader.GetDecimal(1);
                            DateTime purchaseDate = reader.GetDateTime(2);
                            int quantity = reader.GetInt32(3);
                            string product = reader.GetString(4);

                            SaleDetails saleDetails = new SaleDetails(purchaseNumber, total, purchaseDate, quantity, product);
                            salesReport.Add(saleDetails);
                        }
                    }
                }
            }

            return salesReport;
        }

        public async Task<IEnumerable<SalesByDay>> GetWeeklySalesReportAsync(DateTime date)
        {
            if (date == DateTime.MinValue) { throw new ArgumentException("La fecha no puede ser mayor .", nameof(date)); }
            List<SalesByDay> weeklySalesReport = new List<SalesByDay>();
            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

            DateTime startOfWeek = date.AddDays(-(int)date.DayOfWeek);
            string query = @"SELECT DAYNAME(s.purchase_date) AS saleDayOfWeek, COUNT(*) AS saleCount FROM Sales s WHERE YEARWEEK(s.purchase_date) = YEARWEEK(@startDate) GROUP BY saleDayOfWeek ORDER BY saleDayOfWeek;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startOfWeek);
                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string saleDayOfWeek = reader.GetString(0);
                            int saleCount = reader.GetInt32(1);
                            SalesByDay salesByDay = new SalesByDay(saleDayOfWeek, saleCount);
                            weeklySalesReport.Add(salesByDay);
                        }
                    }
                }
            }
            return weeklySalesReport;
        }
    }
}
