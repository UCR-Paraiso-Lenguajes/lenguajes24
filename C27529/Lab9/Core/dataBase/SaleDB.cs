using System;
using MySqlConnector;
using storeApi.Models;

namespace storeApi.Database
{
    public sealed class SaleDB
    {
        public void Save(Sale sale)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=mysql;Uid=root;Pwd=123456;"))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string insertQuery = @"
                            use store;

                            INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                            VALUES (@purchase_date, @total, @payment_method, @purchase_number);";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                            command.Parameters.AddWithValue("@total", sale.Amount);
                            command.Parameters.AddWithValue("@payment_method", sale.PaymentMethod);
                            command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Console.WriteLine("Sale saved to database successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("An error occurred while saving sale to database: " + ex.Message);
                    }
                }
            }
        }
    }
}
