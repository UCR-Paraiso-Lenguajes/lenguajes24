using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using StoreAPI.models;

namespace StoreAPI.Database;
public sealed class StoreDB
{
    internal static void CreateMysql()
    {
    
      var products = new List<Product>{

        new Product
        {
            Name = "Cinder",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768889_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9500,
            Id = 1
        },

       new Product
        {
            Name = "Scarlet",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768896_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9500,
            Id = 2
        },

       new Product
        {
            Name = "Cress",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768902_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9500,
            Id = 3
        },

        new Product
        {
            Name = "Winter",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768926_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 4
        },

        new Product
        {
            Name = "Fairest",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250774057_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 8700,
            Id = 5
        },

        new Product
        {
            Name = "La Sociedad de la Nieve",
            Author = "Pablo Vierci",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786070794162_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 12800,
            Id = 6
        },

        new Product
        {
            Name = "En Agosto nos vemos",
            Author = "Gabriel García Márquez",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073911290_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 14900,
            Id = 7
        },

        new Product
        {
            Name = "El estrecho sendero entre deseos",
            Author = "Patrick Rothfuss",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9789585457935_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 12800,
            Id = 8
        },

        new Product
        {
            Name = "Alas de Sangre",
            Author = "Rebecca Yarros",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788408279990_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 19800,
            Id = 9
        },

       new Product
        {
            Name = "Corona de Medianoche",
            Author = "Sarah J. Mass",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073143691_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 15800,
            Id = 10
        },

       new Product
        {
            Name = "Carta de Amor a los Muertos",
            Author = "Ava Dellaira",
            ImgUrl = "https://m.media-amazon.com/images/I/41IETN4YxGL._SY445_SX342_.jpg",
            Price = 8900,
            Id = 11
        },

        new Product
        {
            Name = "Alicia en el país de las Maravillas",
            Author = "Lewis Carrol",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788415618713_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 7900,
            Id = 0
        }
      };



    string connectionString = "Server=localhost;Database=mysql;Port=3306;Uid=root;Pwd=123456;";
        using (var connection = new MySqlConnection(connectionString))
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
                    ('2024-04-11 13:45:00', 100.50, 1, '98765');";


            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                int result = command.ExecuteNonQuery();
bool dbNoCreated = result < 0;
                if (dbNoCreated)
                    throw new Exception("Error creating the bd");
            }

            // Begin a transaction
            using (var transaction = connection.BeginTransaction())
{
    try
    {
        foreach (Product product in products)
        {
            string insertProductQuery = @"
                            INSERT INTO products (name, price)
                            VALUES (@name, @price);";

            using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
            {
                insertCommand.Parameters.AddWithValue("@name", product.Name);
                insertCommand.Parameters.AddWithValue("@price", product.Price);
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
        }
    }
}