using Core;
using MySqlConnector;
using storeApi.Models.Data;
using storeApi.Models;
namespace storeApi.DataBase
{
    public sealed class SaleDataBase
    {
        public async Task SaveAsync(Sale sale)
        {
            if (sale == null) { throw new ArgumentNullException( $"El parámetro {nameof(sale)} no puede ser nulo."); }
            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            command.Transaction = transaction;

                            command.CommandText = @"
                                    INSERT INTO sales (purchase_date, total, payment_method, purchase_id)
                                    VALUES (@purchase_date, @total, @payment_method, @purchase_id);";

                            command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                            command.Parameters.AddWithValue("@total", sale.Amount);
                            command.Parameters.AddWithValue("@payment_method", (int)sale.PaymentMethod);
                            command.Parameters.AddWithValue("@purchase_id", sale.PurchaseNumber);

                            await command.ExecuteNonQueryAsync();

                            command.Parameters.Clear(); // limpia los parametros 

                            foreach (var productId in sale.Products)
                            {
                                command.CommandText = @"
                                    INSERT INTO linesSales (purchase_id, product_id, quantity, price)
                                    VALUES (@purchase_id, @product_id, @quantity, @price);";

                                command.Parameters.AddWithValue("@purchase_id", sale.PurchaseNumber);
                                command.Parameters.AddWithValue("@product_id", productId.id);
                                command.Parameters.AddWithValue("@quantity", productId.cant);
                                command.Parameters.AddWithValue("@price", productId.price);

                                await command.ExecuteNonQueryAsync();
                                command.Parameters.Clear(); // limpia los parametros para cada producto nuevo
                            }
                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }//transaction
                }//command
            }//connection
        }//save

        public async Task<IEnumerable<SalesData>> GetSalesByDateAsync(DateTime date)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue) { throw new ArgumentException($"La variable {nameof(date)} no puede ser defualt."); }
            List<SalesData> salesList = new List<SalesData>();
            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();
                string query = @"
                    SELECT s.purchase_date, s.total, s.purchase_id, SUM(ls.quantity) AS total_quantity, GROUP_CONCAT(CONCAT(p.name, ':', ls.quantity)) AS products
                    FROM sales s
                    INNER JOIN linesSales ls ON s.purchase_id = ls.purchase_id
                    INNER JOIN products p ON ls.product_id = p.id
                    WHERE DATE(s.purchase_date) = DATE(@purchase_date)
                    GROUP BY s.purchase_id, s.purchase_date, s.total";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@purchase_date", date);

                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime purchaseDate = reader.GetDateTime("purchase_date");
                            decimal total = reader.GetDecimal("total");
                            string purchaseId = reader.GetString("purchase_id");
                            int totalQuantity = reader.GetInt32("total_quantity");
                            string productsString = reader.GetString("products");
                            List<ProductQuantity> products = productsString.Split(',')
                                .Select(p => new ProductQuantity(p.Split(':')[0], int.Parse(p.Split(':')[1])))
                                .ToList();
                            SalesData salesData = new SalesData(purchaseDate, purchaseId, total, totalQuantity, products);
                            salesList.Add(salesData);
                        }
                    }
                }
            }
            return salesList;
        }

        public async Task<IEnumerable<SaleAnnotation>> GetSalesWeekAsync(DateTime date)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue) { throw new ArgumentException($"La variable {nameof(date)} no puede ser defualt."); }
            List<SaleAnnotation> salesByDay = new List<SaleAnnotation>();
            using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();
                string query = @"
                            SELECT DAYNAME(s.purchase_date) AS day, SUM(s.total) AS total
                            FROM sales s
                            WHERE YEARWEEK(s.purchase_date) = YEARWEEK(@purchase_date)
                            GROUP BY DAYNAME(s.purchase_date)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@purchase_date", date);

                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string dayName = reader.GetString("day");
                            DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayName, true);
                            decimal total = reader.GetDecimal("total");
                            salesByDay.Add(new SaleAnnotation(dayOfWeek, total));
                        }
                    }
                }
            }
            return salesByDay;
        }
    }//class
}
