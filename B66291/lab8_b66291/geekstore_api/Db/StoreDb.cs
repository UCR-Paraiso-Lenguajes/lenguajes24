using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace geekstore_api.DataBase
{
    public sealed class StoreDb
        {
            internal static void CreateMysql()
        {
            string connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Orden (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    total DECIMAL(10, 2) NOT NULL,
                    purchase_number VARCHAR(50) NOT NULL,
                    payment_method INT NOT NULL
                    );";

                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
