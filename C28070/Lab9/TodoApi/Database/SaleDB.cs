
using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using TodoApi.Models;


namespace TodoApi.Database {
public sealed class SaleDB
{
    public void save(Sale sale)
    {
        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mysql;Port=3306;Uid=root;Pwd=123456;"))

        { connection.Open();

                using (var transaction = connection.BeginTransaction())
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
                            insertCommand.ExecuteNonQuery();
                        }

                        string insertQueryLines = @"
                        use store;
                        INSERT INTO saleLines (productId, purchaseNumber, price)
                        VALUES (@product_Id, @purchase_Number, @product_Price);";

                        foreach (var product in sale.Products)
                        {
                            using (var insertCommandLines = new MySqlCommand(insertQueryLines, connection, transaction))
                            {
                                insertCommandLines.Parameters.AddWithValue("@product_Id", product.id);
                                insertCommandLines.Parameters.AddWithValue("@purchase_Number", sale.PurchaseNumber);
                                insertCommandLines.Parameters.AddWithValue("@product_Price", product.price);
                                insertCommandLines.ExecuteNonQuery();
                            }
                        }

                    
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                      
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}