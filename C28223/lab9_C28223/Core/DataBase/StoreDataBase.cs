using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace storeApi.DataBase
{
    public sealed class StoreDataBase
    {
        public static void CreateMysql()
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
                                    DROP DATABASE IF EXISTS store;
                                    CREATE DATABASE store;
                                    USE store;
                                    CREATE TABLE IF NOT EXISTS products (
                                     id INT AUTO_INCREMENT PRIMARY KEY,
                                     name VARCHAR(100),
                                     description VARCHAR(255),
                                     price DECIMAL(10, 2),
                                     imageURL VARCHAR(255)
                                 );
                                 CREATE TABLE IF NOT EXISTS paymentMethod (
                                     id INT PRIMARY KEY,
                                     method_name VARCHAR(50)
                                 );
                                 CREATE TABLE IF NOT EXISTS sales (
                                     purchase_date DATETIME NOT NULL,
                                     total DECIMAL(10, 2) NOT NULL,
                                     payment_method INT,
                                     purchase_id VARCHAR(30) NOT NULL PRIMARY KEY
                                 );
                                 CREATE TABLE IF NOT EXISTS linesSales(
                                     id INT AUTO_INCREMENT PRIMARY KEY,
                                     purchase_id VARCHAR(30) NOT NULL,
                                     product_id INT,
                                     quantity INT,
                                     price DECIMAL(10, 2),
                                     FOREIGN KEY (purchase_id) REFERENCES sales(purchase_id),
                                     FOREIGN KEY (product_id) REFERENCES products(id)
                                 );
                                 INSERT INTO paymentMethod (id, method_name)
                                 VALUES (0, 'Efectivo'), (1, 'Sinpe');";

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
                InsertProducts();
            }
        }
        internal static void InsertProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    name = "Producto 1",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://m.media-amazon.com/images/I/71Cco7OaVxL.__AC_SX300_SY300_QL70_FMwebp_.jpg"
                },
                new Product
                {
                    name = "Producto 2",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 3",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 4",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 5",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 6",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 7",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 8",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 9",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 10",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 11",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    name = "Producto 12",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                }
            };
            string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var product in products)
                        {
                            string insertQuery = @"
                                INSERT INTO products (name, description, price, imageURL)
                                VALUES (@name, @description, @price, @imageURL)";

                            using (var command = new MySqlCommand(insertQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@name", product.name);
                                command.Parameters.AddWithValue("@description", product.description);
                                command.Parameters.AddWithValue("@price", product.price);
                                command.Parameters.AddWithValue("@imageURL", product.imageURL);

                                command.ExecuteNonQuery();
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
        }// InsertProducts();

        public static List<Product> GetProductsFromDB()
        {
            List<Product> products = new List<Product>();
            string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, name, description, price, imageURL FROM products";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                id = reader.GetInt32("id"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                price = reader.GetDecimal("price"),
                                imageURL = reader.GetString("imageURL")
                            });
                        }
                    }
                }
            }
            return products;
        }// GetProductsFromDB()

    }// storeDatabase
}
