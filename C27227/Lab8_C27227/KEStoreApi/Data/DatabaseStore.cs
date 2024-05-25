using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace KEStoreApi.Data;
public sealed class DatabaseStore{
   
    public DatabaseStore(){

    }
     
 internal static void Store_MySql()
    {
        string connectionString = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    string createTableQuery = @"
                        DROP DATABASE IF EXISTS store;
                        CREATE DATABASE store;
                        USE store;
                        CREATE TABLE IF NOT EXISTS products (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            name VARCHAR(100),
                            price DECIMAL(10, 2),
                            ImageUrl VARCHAR(250)
                        );

                        CREATE TABLE IF NOT EXISTS Sales (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            total DECIMAL(10, 2) NOT NULL,
                            purchase_date DATETIME NOT NULL,
                            purchaseNumber VARCHAR(50) NOT NULL,
                            payment_method INT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS Lines_Sales (
                            id_line INT AUTO_INCREMENT PRIMARY KEY,
                            id_Sale INT NOT NULL,
                            id_Product INT NOT NULL,
                            price DECIMAL(10,2) NOT NULL,
                            FOREIGN KEY (id_Sale) REFERENCES Sales(id),
                            FOREIGN KEY (id_Product) REFERENCES products(id)
                        );
                    ";

                    using (var command = new MySqlCommand(createTableQuery, connection, transaction))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                        {
                            throw new Exception("Error creating the database");
                        }
                    }

                    // Commit the transaction if everything succeeds
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
