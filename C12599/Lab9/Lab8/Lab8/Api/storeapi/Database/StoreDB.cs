using System;
using System.Collections.Generic;
using MySqlConnector;

namespace storeapi
{
    public sealed class StoreDB
    {
        private readonly string _connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

        public StoreDB()
        {
        }

        public static void CreateMysql()
        {
            var storeDB = new StoreDB(); 

      
            using (var connection = new MySqlConnection(storeDB._connectionString))
            {
                connection.Open();

                string dropTableQuery = "DROP TABLE IF EXISTS products";

                using (var dropTableCommand = new MySqlCommand(dropTableQuery, connection))
                {
                    dropTableCommand.ExecuteNonQuery();
                }
            }

            // Crear la tabla 'products'
            using (var connection = new MySqlConnection(storeDB._connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE products (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100),
                        price DECIMAL(10, 2),
                        image VARCHAR(255),
                        description VARCHAR(255)
                    )";

                using (var createTableCommand = new MySqlCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }

            // Insertar nuevos productos
            var products = new List<Product>();

            for (int i = 1; i <= 12; i++)
            {
                products.Add(new Product
                {
                    Name = $"Product {i}",
                    ImageUrl = $"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
                    Price = 10.99m * i,
                    Description = $"Description of Product {i}"
                });
            }

            using (var connection = new MySqlConnection(storeDB._connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (Product product in products)
                        {
                            string insertProductQuery = @"
                                INSERT INTO products (name, price, description, image)
                                VALUES (@name, @price, @description, @image);";

                            using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@name", product.Name);
                                insertCommand.Parameters.AddWithValue("@price", product.Price);
                                insertCommand.Parameters.AddWithValue("@description", product.Description);
                                insertCommand.Parameters.AddWithValue("@image", product.ImageUrl);

                                insertCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error inserting products into database: {ex.Message}");
                    }
                }
            }
        }

        public static List<string[]> RetrieveDatabaseInfo()
        {
            List<string[]> databaseInfo = new List<string[]>();
            string connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM products";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int fieldCount = reader.FieldCount;
                            string[] row = new string[fieldCount];
                            for (int i = 0; i < fieldCount; i++)
                            {
                                row[i] = reader.GetValue(i).ToString();
                            }
                            databaseInfo.Add(row);
                        }
                    }
                }
            }

            return databaseInfo;
        }
    }
    
 }

