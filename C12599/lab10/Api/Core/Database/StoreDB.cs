using System;
using System.Collections.Generic;
using MySqlConnector;
using storeapi.Models;
using core;

namespace storeapi.Database
{
    public sealed class StoreDB
    {
        public static void CreateMysql()
        {
            var categories = new Categories();
            var products = new List<Product>();
            Random random = new Random();

            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                // Verificar el número actual de productos en la tabla
                string countProductsQuery = "SELECT COUNT(*) FROM products";

                using (var countProductsCommand = new MySqlCommand(countProductsQuery, connection))
                {
                    int currentProductCount = Convert.ToInt32(countProductsCommand.ExecuteScalar());

                    if (currentProductCount >= 14)
                    {
                        throw new InvalidOperationException("No se pueden insertar más productos. Ya se han insertado 12 productos.");
                    }
                }

                // Continuar con la creación de la tabla y la inserción de productos
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100) not null,
                        price DECIMAL(10, 2) not null,
                        image VARCHAR(255) not null,
                        description VARCHAR(255) not null,
                        category INT not null
                    )";

                using (var createTableCommand = new MySqlCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }

                for (int i = 1; i <= 14; i++)
                {
                    int randomIndex = random.Next(0, categories.ListCategories.Count); // Obtener un índice aleatorio válido
                    int randomCategoryId = categories.ListCategories[randomIndex].Id; // Obtener el ID de la categoría en el índice aleatorio
                    products.Add(new Product
                    {
                        Name = $"Product {i}",
                        ImageUrl = $"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
                        Price = 10.99m * i,
                        Description = $"Description of Product {i}",
                        CategoryID = randomCategoryId // Asignar el ID de la categoría aleatoria al producto
                    });
                }

                if (products.Count == 0)
                {
                    throw new ArgumentException("La lista de productos no puede estar vacía.", nameof(products));
                }

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (Product product in products)
                        {
                            ValidateProductForInsert(product);

                            string insertProductQuery = @"
                                INSERT INTO products (name, price, description, image, category)
                                VALUES (@name, @price, @description, @image, @category)";

                            using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@name", product.Name);
                                insertCommand.Parameters.AddWithValue("@price", product.Price);
                                insertCommand.Parameters.AddWithValue("@description", product.Description);
                                insertCommand.Parameters.AddWithValue("@image", product.ImageUrl);
                                insertCommand.Parameters.AddWithValue("@category", product.CategoryID);
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
            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
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

        private static void ValidateProductForInsert(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("El nombre del producto no puede ser nulo o vacío.", nameof(product.Name));
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("El precio del producto no puede ser negativo.", nameof(product.Price));
            }

            if (product.CategoryID <= 0)
            {
                throw new ArgumentException("El ID de categoría del producto debe ser mayor que cero.", nameof(product.CategoryID));
            }
        }
    }
}
