using System;
using System.Data.Common;
using System.IO.Compression;
using Core;
using MySqlConnector;
using TodoApi.models;

namespace TodoApi.db;
public sealed class StoreDB
{
    public StoreDB()
    {


    }

    public static void CreateMysql()
    {
        var products = new List<Product>();

        // Generate 30 sample products
        for (int i = 1; i <= 30; i++)
        {
            products.Add(new Product
            {
                Name = $"Product {i}",
                ImageUrl = $"https://example.com/image{i}.jpg",
                Price = 10.99m * i,
                Description = $"Description of Product {i}",
                Uuid = Guid.NewGuid()
            });
        }
        Console.WriteLine(Storage.Instance.ConnectionString);
        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            connection.Open();

            // Create the products table if it does not exist
            string createTableQuery = @"
                DROP DATABASE IF EXISTS store;
                CREATE DATABASE store;
                use store;
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
                );
                
                INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                VALUES 
                    ('2024-04-11 10:00:00', 50.00, 1, '12345'),
                    ('2024-04-11 11:30:00', 75.20, 2, '54321'),
                    ('2024-04-11 13:45:00', 100.50, 1, '98765');

                ";


            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                int result = command.ExecuteNonQuery();
                bool dbNoCreated = result < 0;
                if(dbNoCreated)
                    throw new Exception("Error creating the bd");
            }

            // Begin a transaction
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Insert 30 products into the table
                    int i =0;
                    foreach(Product prodduct in products)
                    {
                        i++;
                        string productName = $"Product {i}";
                        decimal productPrice = i * 10.0m;

                        string insertProductQuery = @"
                            INSERT INTO products (name, price)
                            VALUES (@name, @price);";

                        using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", productName);
                            insertCommand.Parameters.AddWithValue("@price", productPrice);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    // Commit the transaction if all inserts are successful
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    throw;
                }
            }
        
            // Create command for creating stored procedure
            using (MySqlCommand createProcedureCommand = connection.CreateCommand())
            {
                createProcedureCommand.CommandText = @"
                    CREATE PROCEDURE InsertSales()
                    BEGIN
                        INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                        VALUES ('2024-04-25 10:00:00', 100.00, 1, 'ABC123'),
                               ('2024-04-26 11:30:00', 150.00, 2, 'XYZ456');
                    END
                   ";
                createProcedureCommand.ExecuteNonQuery();
                Console.WriteLine("Stored procedure InsertSales created successfully.");
            }
        
        }
    }
}