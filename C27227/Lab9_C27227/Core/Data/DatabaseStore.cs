using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace KEStoreApi.Data
{
    public sealed class DatabaseStore
    {
        public DatabaseStore()
        {

        }

        public static void Store_MySql()
        {
                 var products = new List<Product>
{
    new Product
    {
        Id = 1,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 2,
        Name = "PlayStation 4",
        Price = 212,
        ImageUrl = "https://media.wired.com/photos/62eabb0e58719fe5c578ec7c/master/pass/What-To-Do-With-Old-PS4-Gear.jpg"
    },
    new Product
    {
        Id = 3,
        Name = "Hogwarts Legacy",
        Price = 47,
        ImageUrl = "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/software/switch/70070000019147/8d6950111fb9a0ece31708dcd6ac893f93c012bc585a6a09dfd986d56ab483d1"
    },
    new Product
    {
        Id = 4,
        Name = "Keyboard Corsair",
        Price = 79,
        ImageUrl = "https://i.ytimg.com/vi/KhVg_7WqCaA/maxresdefault.jpg"
    },
    new Product
    {
        Id = 5,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 7,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 8,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 9,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 10,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 11,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 12,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 13,
        Name = "Logitech Superlight",
        Price = 126,
        ImageUrl = "https://cdn.mos.cms.futurecdn.net/uFsmnUYaPrVA48v9ubwhCV.jpg"
    },
    new Product
    {
        Id = 14,
        Name = "AMD RYZEN 9 7950X3D",
        Price = 626,
        ImageUrl = "https://www.techspot.com/articles-info/2636/images/2023-02-27-image-14.jpg"
    },
     new Product
    {
        Id = 15,
        Name = "GIGABYTE GeForce RTX 4060 AERO OC 8G Graphics Card",
        Price = 319,
        ImageUrl = "https://static.tweaktown.com/news/9/0/90778_02_gigabyte-accidentally-confirms-geforce-rtx-4070-12gb-and-4060-8gb_full.jpg"
    }
};

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
                                ImageUrl VARCHAR(255)
                            );

                            CREATE TABLE IF NOT EXISTS Sales (
                                purchaseNumber VARCHAR(50) NOT NULL Primary Key,
                                total DECIMAL(10, 2) NOT NULL,
                                purchase_date DATETIME NOT NULL,
                                payment_method INT NOT NULL
                            );

                            CREATE TABLE IF NOT EXISTS Lines_Sales (
                                id_line INT AUTO_INCREMENT PRIMARY KEY,
                                id_Sale VARCHAR(50) NOT NULL,
                                id_Product INT NOT NULL,
                                price DECIMAL(10,2) NOT NULL,
                                FOREIGN KEY (id_Sale) REFERENCES Sales(purchaseNumber),
                                FOREIGN KEY (id_Product) REFERENCES products(id)
                            );

                            CREATE TABLE IF NOT EXISTS paymentMethod (
                                id INT PRIMARY KEY,
                                description VARCHAR(50)
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

                        foreach (Product product in products)
                        {
                            string insertProductQuery = @"
                                INSERT INTO products(name, price, ImageUrl)
                                VALUES(@name, @price, @imageUrl);";

                            using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@name", product.Name);
                                insertCommand.Parameters.AddWithValue("@price", product.Price);
                                insertCommand.Parameters.AddWithValue("@imageUrl", product.ImageUrl);
                                insertCommand.ExecuteNonQuery();
                            }
                        }

                        string insertPaymentQuery = @"
                            INSERT INTO paymentMethod (id, description)
                            VALUES (@id, @description);";

                        using (var insertPaymentCommand = new MySqlCommand(insertPaymentQuery, connection, transaction))
                        {
                            insertPaymentCommand.Parameters.AddWithValue("@id", 0);
                            insertPaymentCommand.Parameters.AddWithValue("@description", "Cash");
                            insertPaymentCommand.ExecuteNonQuery();
                        }

                        using (var insertSinpeCommand = new MySqlCommand(insertPaymentQuery, connection, transaction))
                        {
                            insertSinpeCommand.Parameters.AddWithValue("@id", 1);
                            insertSinpeCommand.Parameters.AddWithValue("@description", "Sinpe");
                            insertSinpeCommand.ExecuteNonQuery();
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

        public static List<Product> GetProductsFromDB()
        {
            List<Product> products = new List<Product>();
            string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, name, price, ImageUrl FROM products";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Price = reader.GetDecimal("price"),
                                ImageUrl = reader.GetString("ImageUrl")
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}
