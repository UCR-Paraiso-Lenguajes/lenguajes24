using System;
using System.Data.Common;
using System.IO.Compression;
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

                string insertQuery = @"
                    use store;

                    INSERT INTO sales ( purchase_date, total, payment_method, purchase_number)
                    VALUES ( @purchase_date, @total, @payment_method, @purchase_number);";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                    command.Parameters.AddWithValue("@total", sale.Amount);
                    command.Parameters.AddWithValue("@payment_method", sale.PaymentMethod);
                    command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                    command.ExecuteNonQuery();
                }
                 Console.WriteLine("Cart saved to database successfully.");
            }
        }
    }
}