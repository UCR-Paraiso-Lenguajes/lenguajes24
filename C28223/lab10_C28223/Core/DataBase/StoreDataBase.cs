using System;
using System.Data.Common;
using System.IO.Compression;
using Core;
using MySqlConnector;
using storeApi.Models;
using storeApi.Models.Data;

namespace storeApi.DataBase
{
    public sealed class StoreDataBase
    {
        public static void CreateMysql()
        {
            Categories categoryList = new Categories();
            var products = new List<Product>
            {
                new Product
                {
                    name = "Producto 1",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://m.media-amazon.com/images/I/71Cco7OaVxL.__AC_SX300_SY300_QL70_FMwebp_.jpg",
                    category=categoryList.GetCategories().ToList()[0] //aqui lo correcto es llamar a la categoria por el id y asignarle el struct, por el momento esta random
                },
                new Product
                {
                    name = "Producto 2",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[1]
                },
                new Product
                {
                    name = "Producto 3",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[1]
                },
                new Product
                {
                    name = "Producto 4",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[2]
                },
                new Product
                {
                    name = "Producto 5",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[2]
                },
                new Product
                {
                    name = "Producto 6",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[2]
                },
                new Product
                {
                    name = "Producto 7",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[3]
                },
                new Product
                {
                    name = "Producto 8",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[5]
                },
                new Product
                {
                    name = "Producto 9",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[3]
                },
                new Product
                {
                    name = "Producto 10",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[3]
                },
                new Product
                {
                    name = "Producto 11",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[4]
                },
                new Product
                {
                    name = "Producto 12",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg",
                    category=categoryList.GetCategories().ToList()[5]
                }
            };
            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                connection.Open();

                // Create tables if it does not exist
                string createTableQuery = @"
                                    DROP DATABASE IF EXISTS store;
                                    CREATE DATABASE store;
                                    USE store;
                                    CREATE TABLE IF NOT EXISTS products (
                                     id INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
                                     name VARCHAR(100) NOT NULL,
                                     description VARCHAR(255) NOT NULL,
                                     price DECIMAL(10, 2) NOT NULL,
                                     imageURL VARCHAR(255) NOT NULL,
                                     categoryID INT NOT NULL
                                 );
                                 CREATE TABLE IF NOT EXISTS paymentMethod (
                                     id INT PRIMARY KEY NOT NULL,
                                     method_name VARCHAR(50) NOT NULL
                                 );
                                 CREATE TABLE IF NOT EXISTS sales (
                                     purchase_date DATETIME NOT NULL,
                                     total DECIMAL(10, 2) NOT NULL,
                                     payment_method INT NOT NULL,
                                     purchase_id VARCHAR(30) NOT NULL PRIMARY KEY,
                                     FOREIGN KEY (payment_method) REFERENCES paymentMethod(id)
                                 );
                                 CREATE TABLE IF NOT EXISTS linesSales(
                                     id INT AUTO_INCREMENT PRIMARY KEY,
                                     purchase_id VARCHAR(30) NOT NULL,
                                     product_id INT NOT NULL,
                                     quantity INT NOT NULL,
                                     price DECIMAL(10, 2) NOT NULL,
                                     FOREIGN KEY (purchase_id) REFERENCES sales(purchase_id),
                                     FOREIGN KEY (product_id) REFERENCES products(id)
                                 );
                                 INSERT INTO paymentMethod (id, method_name)
                                 VALUES (0, 'Efectivo'), (1, 'Sinpe');
                                 
                       INSERT INTO sales (purchase_date, total, payment_method, purchase_id)
                         VALUES 
                             ('2024-04-01 10:00:00', 150.00, 0, 'BVS01'),
                             ('2024-04-01 10:00:00', 150.00, 0, 'PUR099'),
                             ('2024-04-02 12:00:00', 200.00, 1, 'PUR02'),
                             ('2024-04-04 16:00:00', 400.00, 1, 'PUR04'),
                             ('2024-04-05 18:00:00', 250.00, 0, 'BVS05'),
                             ('2024-04-06 20:00:00', 180.00, 1, 'PUR06'),
                             ('2024-04-07 22:00:00', 350.00, 0, 'PUR07'),
                             ('2024-04-08 10:00:00', 180.00, 0, 'PUR08'),
                             ('2024-04-09 12:00:00', 220.00, 1, 'PUR09'),
                             ('2024-04-10 14:00:00', 320.00, 0, 'PUR10'),
                             ('2024-04-11 16:00:00', 420.00, 1, 'PUR11'),
                             ('2024-04-12 18:00:00', 270.00, 0, 'PUR12'),
                             ('2024-04-13 20:00:00', 200.00, 1, 'PUR13'),
                             ('2024-04-14 22:00:00', 380.00, 0, 'PUR14');";
                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    int result = command.ExecuteNonQuery();
                    bool dbNotCreated = result < 0;
                    if (dbNotCreated)
                    {
                        throw new Exception("Error creating the database");
                    }
                }
            }
            using (var connectionMyDb = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                connectionMyDb.Open();
                using (var transaction = connectionMyDb.BeginTransaction())
                {
                    try
                    {
                        foreach (var product in products)
                        {
                            string insertQuery = @"
                                INSERT INTO products (name, description, price, imageURL, categoryID)
                                VALUES (@name, @description, @price, @imageURL, @categoryID)";

                            using (var command = new MySqlCommand(insertQuery, connectionMyDb, transaction))
                            {
                                command.Parameters.AddWithValue("@name", product.name);
                                command.Parameters.AddWithValue("@description", product.description);
                                command.Parameters.AddWithValue("@price", product.price);
                                command.Parameters.AddWithValue("@imageURL", product.imageURL);
                                command.Parameters.AddWithValue("@categoryID", product.category.CategoryID);
                                command.ExecuteNonQuery();
                            }
                        }
                        string createLinesQuery = @"
                              INSERT INTO linesSales (purchase_id, product_id, quantity, price)
                              VALUES 
                                  ('BVS01', 1, 2, 50.00),
                                  ('BVS01', 2, 1, 30.00),
                                  ('PUR099', 1, 2, 50.00),
                                  ('PUR099', 2, 1, 30.00),
                                  ('PUR02', 3, 3, 20.00),
                                  ('PUR04', 1, 2, 50.00),
                                  ('PUR04', 3, 1, 30.00),
                                  ('PUR04', 4, 2, 80.00),
                                  ('BVS05', 2, 1, 40.00),
                                  ('BVS05', 3, 3, 60.00),
                                  ('PUR06', 1, 2, 50.00),
                                  ('PUR06', 2, 1, 30.00),
                                  ('PUR07', 3, 1, 20.00),
                                  ('PUR07', 4, 2, 90.00),
                                  ('PUR08', 1, 2, 40.00),
                                  ('PUR08', 2, 1, 30.00),
                                  ('PUR09', 3, 3, 20.00),
                                  ('PUR10', 1, 1, 70.00),
                                  ('PUR10', 2, 2, 50.00),
                                  ('PUR10', 4, 1, 100.00),
                                  ('PUR11', 1, 2, 50.00),
                                  ('PUR11', 3, 1, 30.00),
                                  ('PUR11', 4, 2, 80.00),
                                  ('PUR12', 2, 1, 40.00),
                                  ('PUR12', 3, 3, 60.00),
                                  ('PUR13', 1, 2, 50.00),
                                  ('PUR13', 2, 1, 30.00),
                                  ('PUR14', 3, 1, 20.00),
                                  ('PUR14', 4, 2, 90.00);";
                        using (var insertCommand = new MySqlCommand(createLinesQuery, connectionMyDb, transaction))
                        {
                            insertCommand.ExecuteNonQuery();
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
        internal async Task<IEnumerable<Product>> GetProductsFromDBAsync()
        {
            List<Product> products = new List<Product>();
            var categoryList = new Categories();
            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id, name, description, price, imageURL,categoryID FROM products";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int categoryIdFromDB = reader.GetInt32("categoryID");
                            Category category = categoryList.GetCategoryById(categoryIdFromDB);
                            products.Add(new Product
                            {
                                id = reader.GetInt32("id"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                price = reader.GetDecimal("price"),
                                imageURL = reader.GetString("imageURL"),
                                category = category
                            });
                        }
                    }
                }
            }
            return products;
        }
    }// storeDatabase
}
