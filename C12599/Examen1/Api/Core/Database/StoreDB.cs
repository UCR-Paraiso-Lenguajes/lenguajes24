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
                string[] randomWords = { "amazing", "awesome", "fantastic", "incredible", "superb", "excellent", "wonderful", "marvelous", "brilliant", "fabulous" };
                string[] productNames = { "Gizmo", "Widget", "Contraption", "Gadget", "Appliance", "Device", "Tool", "Instrument", "Machine", "Equipment" };

                for (int i = 1; i <= 14; i++)
                {
                    Category randomCategory = GetRandomCategory(categories);
                    int randomIndex = random.Next(0, categories.ListCategories.Count); // Obtener un índice aleatorio válido

                    // Generar una descripción aleatoria seleccionando algunas palabras al azar
                    string description = $"Description of Product {i}: ";
                    for (int j = 0; j < 1; j++)
                    {
                        int innerRandomWordIndex = random.Next(0, randomWords.Length); // Cambiar el nombre de la variable aquí
                        description += randomWords[innerRandomWordIndex] + " ";
                    }

                    // Seleccionar un nombre aleatorio para el producto
                    int randomWordIndex = random.Next(0, randomWords.Length);
                    int randomNameIndex = random.Next(0, productNames.Length);
                    string productName = $"{productNames[randomNameIndex]} {randomWords[randomWordIndex]}";

                    products.Add(new Product
                    {
                        Name = productName,
                        ImageUrl = $"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
                        Price = 10.99m * i,
                        Description = description.Trim(), // Eliminar el espacio adicional al final
                        Category = randomCategory
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
                                insertCommand.Parameters.AddWithValue("@category", product.Category.Id);
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

        private static Category GetRandomCategory(Categories categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories), "La instancia de 'categories' no puede ser nula.");
            }

            List<Category> categoryList = categories.ListCategories;

            if (categoryList == null || categoryList.Count == 0)
            {
                throw new ArgumentException("La lista de categorías está vacía o es nula.");
            }

            Random random = new Random();
            int index = random.Next(0, categoryList.Count);


            return categoryList[index];
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

        }
    }

}
