/*using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using storeApi.Models;

namespace storeApi.Database
{
    public sealed class LineDB
    {
        public void Save(int productId, string purchaseNumber, decimal price)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3407;Database=mysql;Uid=root;Pwd=123456;"))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string insertQuery = @"
                            use store;
                            INSERT INTO saleLines (productId, purchaseNumber, price)
                            VALUES (@product_Id, @purchase_Number, @product_Price)";

                        using (var insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@product_Id", productId);
                            insertCommand.Parameters.AddWithValue("@purchase_Number", purchaseNumber);
                            insertCommand.Parameters.AddWithValue("@product_Price", price);
                            insertCommand.ExecuteNonQuery();
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

*/
