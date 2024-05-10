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
                            command.Parameters.AddWithValue("@PaymentMethod", sale.PaymentMethod.GetHashCode().ToString());
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
                        insertCommand.Parameters.AddWithValue("@quantity", 1);
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
    }
}