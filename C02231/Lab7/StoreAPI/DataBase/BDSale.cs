using System;
using MySqlConnector;
using StoreAPI;

namespace StoreAPI.Database;

public sealed class BDSale
{

    public void Save(DateTime date, decimal total, int paymentMethod, string orderNumber)
    {

        TableExists();
        using (var connection = new MySqlConnection("Server=localhost;Database=mysql;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO Compras (date, total, paymentMethod, orderNumber)
                VALUES (@date, @total, @PaymentMethod, @orderNumber);";


            using (var command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);

                command.ExecuteNonQuery();
            }
        }
    }


    private void TableExists()
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mysql;Uid=root;Pwd=123456;"))
            {
                connection.Open();
                
                string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Compras (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            total DECIMAL(10, 2) NOT NULL,
                            date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                            orderNumber VARCHAR(255) NOT NULL,
                            PaymentMethod INT
                        );";

                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}

