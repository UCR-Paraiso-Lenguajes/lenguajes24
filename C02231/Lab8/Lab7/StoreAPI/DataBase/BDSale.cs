using System;
using MySqlConnector;
using StoreAPI;
using StoreAPI.models;

namespace StoreAPI.Database;

public sealed class BDSale
{

    public void Save(Sale sale)
    {

      //  TableExists();
        using (var connection = new MySqlConnection("Server=localhost;Database=mysql;Port=3306;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO sales (productIds, date, total, paymentMethod, orderNumber)
                VALUES (@productIds, @date, @total, @PaymentMethod, @orderNumber);";


            using (var command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@productIds", string.Join(",", sale.Products));
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@total", sale.Amount);
                command.Parameters.AddWithValue("@PaymentMethod", sale.PaymentMethod);
                command.Parameters.AddWithValue("@orderNumber", sale.NumberOrder);

                command.ExecuteNonQuery();
            }
        }
    }


 /*   private void TableExists()
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mysql2;Port=3306;Uid=root;Pwd=123456;"))
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
    }*/

}

