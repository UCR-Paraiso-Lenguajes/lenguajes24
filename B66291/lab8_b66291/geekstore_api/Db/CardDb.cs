using MySqlConnector;
using System;

namespace geekstore_api.DataBase
{
    public class CartDb
    {
        private readonly string _connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

        public void almacenarDatos(Sale sale)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string insertQuery = @"
                        INSERT INTO Orden (total, purchase_number, payment_method)
                        VALUES (@total, @purchase_number, @payment_method);";

            using (var command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@total", sale.Amount);
                command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                command.Parameters.AddWithValue("@payment_method", (int)sale.PaymentMethod);
                command.ExecuteNonQuery();
            }
        }
    }
}
