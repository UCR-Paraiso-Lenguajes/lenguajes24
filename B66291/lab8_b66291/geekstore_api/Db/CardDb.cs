using MySqlConnector;
using System;

namespace geekstore_api.CardDb
{
    public class CartDb
    {
        private readonly string _connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

        public void almacenarDatos(decimal total, DateTime date, int purchaseNumber, int paymentMethod)
        {
                // Insertar el carrito en la base de datos
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Orden (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            total DECIMAL(10, 2) NOT NULL,
                            date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                            purchaseNumber INT NOT NULL,
                            Paymethod INT
                        );";
        
                    string insertQuery = @"
                        INSERT INTO Orden (total, date, purchaseNumber, Paymethod)
                        VALUES (@total, @date, @purchaseNumber, @Paymethod);";

                    using (var command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@total", total);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@purchaseNumber", purchaseNumber);
                        command.Parameters.AddWithValue("@Paymethod", paymentMethod);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
}
