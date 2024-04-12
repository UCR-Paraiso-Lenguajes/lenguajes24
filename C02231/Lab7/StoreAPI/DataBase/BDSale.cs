using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using StoreAPI.Model;

namespace StoreAPI.Database;

public sealed class BDSale
{

    public void save(DateTime date, decimal total, int paymentMethod, string purchaseNumber)
    {
        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mysql;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            // Crear tabla Compras con FK de UserId y MetodoPagoId permitiendo nulos
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Compras (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    FechaCompra DATETIME NOT NULL,
                    Monto DECIMAL(10, 2) NOT NULL,
                    MetodoPagoId INT NULL,
                    NumeroCompra VARCHAR(50) NOT NULL,
                    UserId INT NULL,
                    FOREIGN KEY (UserId) REFERENCES Usuarios(Id),
                    FOREIGN KEY (MetodoPagoId) REFERENCES MetodoPago(Id)
                );";

            using (MySqlCommand command = new MySqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mysql;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            // Insertando 3 registros de ejemplo con UserId y MetodoPagoId (pueden ser nulos)
            string insertQuery = @"
                INSERT INTO Compras (FechaCompra, Monto, MetodoPagoId, NumeroCompra, UserId)
                VALUES 
                ('2024-04-11 10:00:00', 50.00, NULL, '12345', 1),
                ('2024-04-11 11:30:00', 75.20, 2, '54321', NULL),
                ('2024-04-11 13:45:00', 100.50, 1, '98765', 3);";

            using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}

