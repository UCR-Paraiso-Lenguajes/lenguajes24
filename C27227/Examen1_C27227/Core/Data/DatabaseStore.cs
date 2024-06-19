using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO.Compression;
using Core;
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
                    ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[4].Id
                },
                new Product
                {
                    Id = 2,
                    Name = "PlayStation 4",
                    Price = 212,
                    ImageUrl = "https://media.wired.com/photos/62eabb0e58719fe5c578ec7c/master/pass/What-To-Do-With-Old-PS4-Gear.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[4].Id
                },
                new Product
                {
                    Id = 3,
                    Name = "Hogwarts Legacy",
                    Price = 47,
                    ImageUrl = "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/software/switch/70070000019147/8d6950111fb9a0ece31708dcd6ac893f93c012bc585a6a09dfd986d56ab483d1",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[4].Id
                },
                new Product
                {
                    Id = 4,
                    Name = "Keyboard Corsair",
                    Price = 79,
                    ImageUrl = "https://i.ytimg.com/vi/KhVg_7WqCaA/maxresdefault.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 5,
                    Name = "Xbox",
                    Price = 349,
                    ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[4].Id
                },
                new Product
                {
                    Id = 7,
                    Name = "Xbox",
                    Price = 349,
                    ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[4].Id
                },
                new Product
                {
                    Id = 8,
                    Name = "Xbox",
                    Price = 349,
                    ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[4].Id
                },
                new Product
                {
                    Id = 9,
                    Name = "Celular",
                    Price = 100,
                    ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 10,
                    Name = "Celular",
                    Price = 100,
                    ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 11,
                    Name = "Celular",
                    Price = 100,
                    ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 12,
                    Name = "Celular",
                    Price = 100,
                    ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 13,
                    Name = "Logitech Superlight",
                    Price = 126,
                    ImageUrl = "https://cdn.mos.cms.futurecdn.net/uFsmnUYaPrVA48v9ubwhCV.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 14,
                    Name = "AMD RYZEN 9 7950X3D",
                    Price = 626,
                    ImageUrl = "https://www.techspot.com/articles-info/2636/images/2023-02-27-image-14.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                },
                new Product
                {
                    Id = 15,
                    Name = "GIGABYTE GeForce RTX 4060 AERO OC 8G Graphics Card",
                    Price = 319,
                    ImageUrl = "https://static.tweaktown.com/news/9/0/90778_02_gigabyte-accidentally-confirms-geforce-rtx-4070-12gb-and-4060-8gb_full.jpg",
                    Quantity = 1,
                    CategoriaId = Categorias.Instance.GetCategorias().ToList()[7].Id
                }
            };
            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

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
                        name VARCHAR(100) NOT NULL,
                        price DECIMAL(10, 2) NOT NULL,
                        ImageUrl VARCHAR(500) NOT NULL,
                        idCategoria INT NOT NULL,
                        isDeleted BOOLEAN DEFAULT FALSE
                    );
                    CREATE TABLE IF NOT EXISTS paymentMethod (
                        id INT PRIMARY KEY,
                        description VARCHAR(50) NOT NULL
                    );
                    INSERT INTO paymentMethod (id, description) VALUES (0, 'Cash');
                    INSERT INTO paymentMethod (id, description) VALUES (1, 'Sinpe');

                    CREATE TABLE IF NOT EXISTS Sales (
                        purchaseNumber VARCHAR(50) NOT NULL PRIMARY KEY,
                        total DECIMAL(10, 2) NOT NULL,
                        purchase_date DATETIME NOT NULL,
                        payment_method INT NOT NULL,
                        FOREIGN KEY (payment_method) REFERENCES paymentMethod(id)
                    );

                    CREATE TABLE IF NOT EXISTS Lines_Sales (
                        id_line INT AUTO_INCREMENT PRIMARY KEY,
                        id_Sale VARCHAR(50) NOT NULL,
                        id_Product INT NOT NULL,
                        quantity INT NOT NULL,
                        price DECIMAL(10, 2) NOT NULL,
                        FOREIGN KEY (id_Sale) REFERENCES Sales(purchaseNumber),
                        FOREIGN KEY (id_Product) REFERENCES products(id)
                    );

                    -- Semana del 7 de abril al 13 de abril
                    INSERT INTO Sales (purchase_date, total, payment_method, purchaseNumber)
                    VALUES 
                        ('2024-04-07 10:00:00', 280.00, 0, 'PUR001'),
                        ('2024-04-08 10:00:00', 480.00, 1, 'PUR002'),
                        ('2024-04-09 12:00:00', 910.00, 1, 'PUR003'),
                        ('2024-04-10 14:00:00', 450.00, 0, 'PUR004'),
                        ('2024-04-11 16:00:00', 340.00, 1, 'PUR005'),
                        ('2024-04-12 18:00:00', 650.00, 0, 'PUR006'),
                        ('2024-04-13 20:00:00', 530.00, 1, 'PUR007');

                    -- Semana del 14 de abril al 20 de abril
                    INSERT INTO Sales (purchase_date, total, payment_method, purchaseNumber)
                    VALUES 
                        ('2024-04-14 10:00:00', 390.00, 0, 'PUR008'),
                        ('2024-04-15 12:00:00', 520.00, 1, 'PUR009'),
                        ('2024-04-16 14:00:00', 930.00, 0, 'PUR010'),
                        ('2024-04-17 16:00:00', 950.00, 1, 'PUR011'),
                        ('2024-04-18 18:00:00', 550.00, 0, 'PUR012'),
                        ('2024-04-19 20:00:00', 460.00, 1, 'PUR013'),
                        ('2024-04-20 22:00:00', 620.00, 0, 'PUR014');
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
                        INSERT INTO products(name, price, ImageUrl, idCategoria)
                        VALUES(@name, @price, @imageUrl, @categoryId);";

                            using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@name", product.Name);
                                insertCommand.Parameters.AddWithValue("@price", product.Price);
                                insertCommand.Parameters.AddWithValue("@imageUrl", product.ImageUrl);
                                insertCommand.Parameters.AddWithValue("@categoryId", product.CategoriaId);
                                insertCommand.ExecuteNonQuery();
                            }
                        }

                        string insertQuery = @"
                    INSERT INTO Lines_Sales (id_Sale, id_Product, quantity, price)
                    VALUES 
                        ('PUR001', 1, 2, 100.00),
                        ('PUR001', 3, 3, 20.00),
                        ('PUR001', 4, 1, 80.00),
                        ('PUR002', 1, 1, 100.00),
                        ('PUR002', 2, 2, 60.00),
                        ('PUR002', 4, 1, 80.00),
                        ('PUR003', 2, 1, 60.00),
                        ('PUR003', 3, 3, 60.00),
                        ('PUR003', 4, 2, 80.00),
                        ('PUR004', 1, 1, 100.00),
                        ('PUR004', 3, 2, 40.00),
                        ('PUR004', 4, 1, 80.00),
                        ('PUR005', 2, 1, 60.00),
                        ('PUR005', 4, 2, 80.00),
                        ('PUR006', 1, 2, 100.00),
                        ('PUR006', 2, 1, 60.00),
                        ('PUR007', 3, 1, 20.00),
                        ('PUR007', 4, 2, 80.00);

                    INSERT INTO Lines_Sales (id_Sale, id_Product, quantity, price)
                    VALUES 
                        ('PUR008', 1, 1, 100.00),
                        ('PUR008', 3, 3, 60.00),
                        ('PUR008', 4, 1, 80.00),
                        ('PUR009', 2, 1, 60.00),
                        ('PUR009', 4, 2, 80.00),
                        ('PUR010', 1, 2, 100.00),
                        ('PUR010', 2, 1, 60.00),
                        ('PUR010', 3, 2, 40.00),
                        ('PUR011', 1, 2, 100.00),
                        ('PUR011', 3, 1, 20.00),
                        ('PUR011', 4, 2, 80.00),
                        ('PUR012', 2, 1, 60.00),
                        ('PUR012', 4, 2, 80.00),
                        ('PUR013', 1, 1, 100.00),
                        ('PUR013', 3, 3, 60.00),
                        ('PUR014', 2, 1, 60.00),
                        ('PUR014', 3, 1, 20.00),
                        ('PUR014', 4, 2, 80.00);
                ";

                        using (var command = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected <= 0)
                            {
                                throw new Exception("Error inserting data into the database");
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
        public static async Task<IEnumerable<Product>> GetProductsFromDBaAsync()
        {
            List<Product> products = new List<Product>();
            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT id, name, price, ImageUrl, idCategoria FROM products WHERE isDeleted = 0";
                using (var command = new MySqlCommand(query, connection))
                {
                    var readerTask = command.ExecuteReaderAsync();
                    using (var reader = await readerTask)
                    {
                        while (await reader.ReadAsync())
                        {
                            int idCategoria = reader.GetInt32("idCategoria");
                            products.Add(new Product
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Price = reader.GetDecimal("price"),
                                ImageUrl = reader.GetString("ImageUrl"),
                                CategoriaId = idCategoria,
                                IsDeleted = false,
                            });
                        }
                    }
                }
            }

            return products;
        }



        public static async Task AddProductAsync(Product product, Store.ProductActionDelegate actions)
        {
            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertProductQuery = @"
                            INSERT INTO products(name, price, ImageUrl, idCategoria)
                            VALUES(@name, @price, @imageUrl, @categoryId);";

                        using (var insertCommand = new MySqlCommand(insertProductQuery, connection, (MySqlTransaction)transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", product.Name);
                            insertCommand.Parameters.AddWithValue("@price", product.Price);
                            insertCommand.Parameters.AddWithValue("@imageUrl", product.ImageUrl);
                            insertCommand.Parameters.AddWithValue("@categoryId", product.CategoriaId);

                            await insertCommand.ExecuteNonQueryAsync();
                        }
                        await transaction.CommitAsync();
                        
                        if (actions != null)
                        {
                            await actions(product);
                        }
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
        public static async Task DeleteProductAsync(int productId)
        {
            string connectionString = DatabaseConfiguration.Instance.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE products SET isDeleted = 1 WHERE id = @productId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }

    internal class Categories
    {
        public static Categories Instance { get; } = new Categories();
    }
}
