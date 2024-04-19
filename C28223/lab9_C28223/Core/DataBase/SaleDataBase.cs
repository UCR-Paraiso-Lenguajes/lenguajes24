using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace storeApi.DataBase
{
    public sealed class SaleDataBase
    {
        public void Save(Sale sale)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=store;Uid=root;Pwd=123456;"))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    using (var transaction = connection.BeginTransaction())
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

                            command.ExecuteNonQuery();
                        
                            command.Parameters.Clear(); // limpia los parametros 

                            foreach (var productId in sale.Products)
                            {
                                command.CommandText = @"
                                    INSERT INTO linesSales (purchase_id, product_id, quantity, price)
                                    VALUES (@purchase_id, @product_id, @quantity, @price);";

                                command.Parameters.AddWithValue("@purchase_id", sale.PurchaseNumber);
                                command.Parameters.AddWithValue("@product_id", productId.id);
                                command.Parameters.AddWithValue("@quantity", 1);
                                command.Parameters.AddWithValue("@price", productId.price);

                                command.ExecuteNonQuery();
                                command.Parameters.Clear(); // limpia los parametros para cada producto nuevo
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }//transaction
                }//command
            }//connection
        }//save
    }//class
}
