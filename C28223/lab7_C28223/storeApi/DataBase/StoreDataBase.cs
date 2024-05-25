using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace storeApi.DataBase
{
    public sealed class StoreDataBase
    {
        internal static void CreateMysql()
        {
            string connectionString = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create the products table if it does not exist
                        string createTableQuery = @"
                            CREATE DATABASE IF NOT EXISTS store;
                            USE store;
                            CREATE TABLE IF NOT EXISTS products (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                name VARCHAR(100),
                                price DECIMAL(10, 2)
                            );
                            CREATE TABLE IF NOT EXISTS sales (
                                Id INT AUTO_INCREMENT PRIMARY KEY,
                                purchase_date DATETIME NOT NULL,
                                total DECIMAL(10, 2) NOT NULL,
                                payment_method INT NOT NULL,
                                purchase_number VARCHAR(50) NOT NULL
                            );";
                        using (var command = new MySqlCommand(createTableQuery, connection, transaction))
                        {
                            int result = command.ExecuteNonQuery();
                            bool dbNotCreated = result < 0;
                            if (dbNotCreated)
                            {
                                throw new Exception("Error creating the database");
                            }
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