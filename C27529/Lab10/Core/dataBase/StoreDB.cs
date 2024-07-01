using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using Core;
using storeApi.Business;
namespace storeApi.db;
public sealed class StoreDB
{
    public StoreDB()
    {
    }

    public static async Task CreateMysql()
    {
        var products = new List<Product>
            {
               new Product
            {
                Id = 1,
                Name = "Producto 1",
                Description = "Audífonos con alta fidelidad",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Fuji/2021/June/Fuji_Quad_Headset_1x._SY116_CB667159060_.jpg",
                Category = new Category.ProductCategory { Id = 1, Name = "Audífonos" }
            },
            new Product
            {
                Id = 2,
                Name = "Producto 2",
                Description = "Control PS4",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Controller2.png",
                Category = new Category.ProductCategory { Id = 2, Name = "Controles" }
            },
            new Product
            {
                Id = 3,
                Name = "Producto 3",
                Description = "PS4 1TB",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Playstation3.jpg",
                Category = new Category.ProductCategory { Id = 3, Name = "Consolas" }
                    },
            new Product
            {
                Id = 4,
                Name = "Producto 4",
                Description = "Crash Bandicoot 4 Switch",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Game.png",
                Category = new Category.ProductCategory { Id = 4, Name = "Videojuegos" }

            },
            new Product
            {
                Id = 5,
                Name = "Producto 5",
                Description = "Mouse Logitech",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_Quad_Mouse.jpg",
                Category =  new Category.ProductCategory { Id = 5, Name = "Mouse" }
                  },
            new Product
            {
                Id = 6,
                Name = "Producto 6",
                Description = "Silla Oficina",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_Quad_Chair.jpg",
                Category = new Category.ProductCategory { Id = 6, Name = "Sillas" }
                   },
            new Product
            {
                Id = 7,
                Name = "Producto 7",
                Description = "Laptop Acer",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Laptop.png",
                Category =     new Category.ProductCategory { Id = 7, Name = "Laptops" }
                     },
            new Product
            {
                Id = 8,
                Name = "Producto 8",
                Description = "Oculus Quest 3",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Oculus2.jpg",
                Category =     new Category.ProductCategory { Id = 8, Name = "RealidadVirtual"}
            },
            new Product
            {
                Id = 9,
                Name = "Producto 9",
                Description = "Teclado mecánico RGB",
                Price = 15000,
                ImageURL = "https://m.media-amazon.com/images/I/61uofDvRldS._AC_UL320_.jpg",
                Category =  new Category.ProductCategory { Id = 9, Name = "Teclados" }
                     },
            new Product
            {
                Id = 10,
                Name = "Producto 10",
                Description = "Monitor gaming 144Hz",
                Price = 30000,
                ImageURL = "https://m.media-amazon.com/images/I/71sPOWyMwVL._AC_UL320_.jpg",
                Category =  new Category.ProductCategory { Id = 10, Name = "Monitores" }
                      },
            new Product
            {
                Id = 11,
                Name = "Producto 11",
                Description = "Cámara DSLR Canon EOS",
                Price = 40000,
                ImageURL = "https://m.media-amazon.com/images/I/61o0MBO9jFL._AC_UL320_.jpg",
                Category = new Category.ProductCategory { Id = 11, Name = "Cámaras" }
                     },
            new Product
            {
                Id = 12,
                Name = "Producto 12",
                Description = "Smartwatch Samsung Galaxy",
                Price = 25000,
                ImageURL = "https://m.media-amazon.com/images/I/711f6KLsMaL._AC_UL320_.jpg",
                Category = new Category.ProductCategory { Id = 12, Name = "Smartwatches" }
            },
            new Product
            {
                Id = 13,
                Name = "Producto 13",
                Description = "Bicicleta de montaña",
                Price = 150000,
                ImageURL = "https://m.media-amazon.com/images/I/817X9TvYQ3L._AC_UL320_.jpg",
                Category =  new Category.ProductCategory { Id = 13, Name = "Bicicletas" }
                         },
            new Product
            {
                Id = 14,
                Name = "Producto 14",
                Description = "Robot aspirador",
                Price = 35000,
                ImageURL = "https://m.media-amazon.com/images/I/619TvTYML3L._AC_UY218_.jpg",
                Category = new Category.ProductCategory { Id = 14, Name = "RobotsAspiradores" }
            },
            new Product
            {
                Id = 15,
                Name = "Producto 15",
                Description = "Proyector de cine en casa",
                Price = 50000,
                ImageURL = "https://m.media-amazon.com/images/I/71iPl3A0ubL._AC_UL320_.jpg",
                Category = new Category.ProductCategory { Id = 15, Name = "Proyectores" }
            },
            new Product
            {
                Id = 16,
                Name = "Producto 16",
                Description = "Cafetera espresso",
                Price = 20000,
                ImageURL = "https://m.media-amazon.com/images/I/71BvCt6eAFL._AC_UL320_.jpg",
                Category =  new Category.ProductCategory { Id = 16, Name = "Cafeteras" }
                },

            new Product
            {
                Id = 17,
                Name = "Auriculares Bluetooth",
                Description = "Auriculares inalámbricos con alta calidad de sonido.",
                Price = 30000,
                ImageURL = "https://m.media-amazon.com/images/I/514TrZ0P5qL._AC_UL320_.jpg",
                Category = new Category.ProductCategory { Id = 1, Name = "Audífonos" }
            },
            new Product
            {
                Id = 18,
                Name = "Mando para PC",
                Description = "Control de juego compatible con PC y consolas.",
                Price = 25000,
                ImageURL = "https://m.media-amazon.com/images/I/71WhtRV7AoL._AC_UL320_.jpg",
                Category = new Category.ProductCategory { Id = 2, Name = "Controles" }
            },
            new Product
            {
                Id = 19,
                Name = "Consola PlayStation 5",
                Description = "Consola de última generación con capacidad de 4K.",
                Price = 60000,
                ImageURL = "https://m.media-amazon.com/images/I/71rWPpdhwgL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 3, Name = "Consolas" }
            },
            new Product
            {
                Id = 20,
                Name = "Videojuego The Last of Us Part II",
                Description = "Aventura épica en un mundo post-apocalíptico.",
                Price = 4000,
                ImageURL = "https://m.media-amazon.com/images/I/818m9WGY0lL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 4, Name = "Videojuegos" }
            },
            new Product
            {
                Id = 21,
                Name = "Mouse inalámbrico",
                Description = "Mouse ergonómico con conexión Bluetooth.",
                Price = 15000,
                ImageURL = "https://m.media-amazon.com/images/I/61N+CzcA8vL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 5, Name = "Mouse" }
            },
            new Product
            {
                Id = 22,
                Name = "Silla gamer",
                Description = "Silla ergonómica diseñada para largas sesiones de juego.",
                Price = 35000,
                ImageURL = "https://m.media-amazon.com/images/I/61Sp94J-hhL._AC_UL320_.jpg",
                Category =     new Category.ProductCategory { Id = 6, Name = "Sillas" }
            },
            new Product
            {
                Id = 23,
                Name = "Laptop HP",
                Description = "Portátil con procesador Intel Core i7 y pantalla Full HD.",
                Price = 80000,
                ImageURL = "https://m.media-amazon.com/images/I/71nIkcFLf9L._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 7, Name = "Laptops" }
            },
            new Product
            {
                Id = 24,
                Name = "Gafas de realidad virtual Oculus Rift",
                Description = "Experimenta la realidad virtual de alta calidad con Oculus Rift.",
                Price = 40000,
                ImageURL = "https://m.media-amazon.com/images/I/61GhF+JUXGL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 8, Name = "RealidadVirtual"}
            },
            new Product
            {
                Id = 25,
                Name = "Teclado mecánico RGB",
                Description = "Teclado mecánico con retroiluminación RGB personalizable.",
                Price = 20000,
                ImageURL = "https://m.media-amazon.com/images/I/61l76udL5rL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 9, Name = "Teclados" }
            },
            new Product
            {
                Id = 26,
                Name = "Monitor ASUS",
                Description = "Monitor de 27 pulgadas con resolución QHD y tasa de actualización de 144Hz.",
                Price = 35000,
                ImageURL = "https://m.media-amazon.com/images/I/71NN-PW+pdL._AC_UL320_.jpg",
                Category =    new Category.ProductCategory { Id = 10, Name = "Monitores" }
            },
            new Product
            {
                Id = 27,
                Name = "Cámara digital Nikon",
                Description = "Cámara DSLR con sensor de 24.2 megapíxeles y grabación de video Full HD.",
                Price = 45000,
                ImageURL = "https://m.media-amazon.com/images/I/61osNWYa4pL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 11, Name = "Cámaras" }
            },
            new Product
            {
                Id = 28,
                Name = "Smartwatch Apple Watch Series 6",
                Description = "Smartwatch con monitorización avanzada de la salud y pantalla siempre activa.",
                Price = 35000,
                ImageURL = "https://m.media-amazon.com/images/I/71eOAQxSbPL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 12, Name = "Smartwatches" }
            },
            new Product
            {
                Id = 29,
                Name = "Desviador trasero bicicleta de montaña",
                Description = "6/7/8 velocidades de montaje de suspensión/montaje directo para bicicleta de montaña MTB.",
                Price = 55000,
                ImageURL = "https://m.media-amazon.com/images/I/61BU+pZTgJL._AC_UL320_.jpg",
                Category =      new Category.ProductCategory { Id = 13, Name = "Bicicletas" }
            },
            new Product
            {
                Id = 30,
                Name = "Robot aspirador iRobot Roomba",
                Description = "Robot aspirador con mapeo inteligente y capacidad de limpieza programada.",
                Price = 60000,
                ImageURL = "https://m.media-amazon.com/images/I/81wCNXD4F0L._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 14, Name = "RobotsAspiradores" }
            },
            new Product
            {
                Id = 31,
                Name = "Proyector Epson",
                Description = "Proyector con resolución Full HD y brillo de 3000 lúmenes.",
                Price = 80000,
                ImageURL = "https://m.media-amazon.com/images/I/51SaUisG5BL._AC_UY218_.jpg",
                Category =     new Category.ProductCategory { Id = 15, Name = "Proyectores" }
            },
            new Product
            {
                Id = 32,
                Name = "Cafetera Nespresso",
                Description = "Cafetera de cápsulas con sistema de extracción rápido y fácil de limpiar.",
                Price = 15000,
                ImageURL = "https://m.media-amazon.com/images/I/71mgVqsz1tL._AC_UL320_.jpg",
                Category =     new Category.ProductCategory { Id = 16, Name = "Cafeteras" }
            }
            };




        using (var connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            await connection.OpenAsync();


            // Drop and create database commands
            string dropDatabaseQuery = "DROP DATABASE IF EXISTS store;";
            string createDatabaseQuery = "CREATE DATABASE store;";
            string useDatabaseQuery = "USE store;";

            using (var dropCommand = new MySqlCommand(dropDatabaseQuery, connection))
            {
                await dropCommand.ExecuteNonQueryAsync();
            }

            using (var createCommand = new MySqlCommand(createDatabaseQuery, connection))
            {
                await createCommand.ExecuteNonQueryAsync();
            }

            using (var useCommand = new MySqlCommand(useDatabaseQuery, connection))
            {
                await useCommand.ExecuteNonQueryAsync();
            }


            string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS paymentMethods (
                paymentId INT PRIMARY KEY,
                paymentName VARCHAR(30) NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS products (
                id INT AUTO_INCREMENT PRIMARY KEY,
                name VARCHAR(100) NOT NULL,
                description  BLOB NOT NULL,
                price DECIMAL(10, 2) NOT NULL,
                imageURL VARCHAR(255) NOT NULL,
                category INT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS sales (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                purchase_date DATETIME NOT NULL,
                total DECIMAL(10, 2) NOT NULL,
                payment_method INT NOT NULL,
                purchase_number VARCHAR(50) NOT NULL UNIQUE,
                FOREIGN KEY (payment_method) REFERENCES paymentMethods(paymentId)
            );

            CREATE TABLE IF NOT EXISTS saleLines (
                productId INT,
                purchaseNumber VARCHAR(50),
                price DECIMAL(10,2) NOT NULL,
                PRIMARY KEY (productId, purchaseNumber),
                FOREIGN KEY (productId) REFERENCES products(id),
                CONSTRAINT fk_purchaseNumber FOREIGN KEY (purchaseNumber) REFERENCES sales(purchase_number)
            );

                      CREATE TABLE IF NOT EXISTS messages (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        content TEXT NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );

                     INSERT INTO paymentMethods (paymentId, paymentName)
                        VALUES 
                        (0, 'Cash'),
                        (1, 'Sinpe');

                INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                VALUES
                    ('2024-06-15 05:20:00', 67.20, 1, 'SA123456789'),
                    ('2024-06-15 14:45:00', 45.80, 0, 'SB987654321'),
                    ('2024-06-15 08:55:00', 35.60, 1, 'SC246813579'),
                    ('2024-06-15 17:30:00', 78.90, 0, 'SD135792468'),
                    ('2024-06-15 10:10:00', 25.50, 0, 'SE987654321'),
                    ('2024-06-15 12:25:00', 50.30, 1, 'SF123456789'),
                    ('2024-06-15 13:15:00', 90.00, 0, 'SG246813579'),
                    ('2024-06-15 07:50:00', 55.25, 1, 'SH135792468'),
                    ('2024-06-15 11:40:00', 40.75, 0, 'SI987654321'),
                    ('2024-06-15 09:05:00', 70.00, 1, 'SJ123456789'),
                    ('2024-06-15 15:20:00', 60.40, 1, 'SX123456789'),
                    ('2024-06-15 08:55:00', 35.75, 0, 'SY987654321'),
                    ('2024-06-15 12:30:00', 80.20, 1, 'SZ246813579'),
                    ('2024-06-15 09:45:00', 25.50, 0, 'SW135792468'),
                    ('2024-06-15 11:00:00', 45.90, 0, 'SV987654321'),
                    ('2024-06-15 14:10:00', 70.80, 1, 'SU123456789'),
                    ('2024-06-15 16:25:00', 50.70, 0, 'ST246813579'),
                    ('2024-06-15 10:35:00', 35.25, 1, 'SS135792468'),
                    ('2024-06-15 13:20:00', 67.00, 0, 'SR987654321'),
                    ('2024-06-15 07:15:00', 40.25, 1, 'SQ123456789'),
                    ('2024-06-15 08:14:00', 90.25, 0, 'SG123456459'),
                    ('2024-06-15 08:23:00', 92.25, 1, 'HG123494459'),
                    ('2024-06-15 08:09:00', 93.25, 0, 'SV123475459'),
                    ('2024-06-15 05:20:00', 67.20, 1, 'SA124456789'),
                    ('2024-06-15 05:20:00', 67.20, 1, 'SA122456789'),
                    ('2024-06-15 08:18:00', 95.25, 1, 'TE123434459'),
                    ('2024-06-15 01:20:00', 67.20, 1, 'SA121456789'),
                    ('2024-06-15 01:50:00', 67.20, 1, 'SA129456789')


                                ";


            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                int result = command.ExecuteNonQuery();
                bool dbNoCreated = result < 0;
                if (dbNoCreated)
                    throw new Exception("Error creating the bd");
            }

            // Begin a transaction
            using (var transaction = await connection.BeginTransactionAsync())
            {
                try
                {
                    foreach (Product product in products)
                    {
                        string insertProductQuery = @"
                                INSERT INTO products (name, description, price, imageURL, category)
                                VALUES (@name, @description ,@price, @imageURL, @category);";

                        using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", product.Name);
                            insertCommand.Parameters.AddWithValue("@description", product.Description);
                            insertCommand.Parameters.AddWithValue("@price", product.Price);
                            insertCommand.Parameters.AddWithValue("@imageURL", product.ImageURL);
                            insertCommand.Parameters.AddWithValue("@category", product.Category.Id.ToString());
                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
    public static async Task<List<Product>> GetProductsAsync()
    {
        List<Product> products = new List<Product>();
        Category category = new Category();
        using (MySqlConnection connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            await connection.OpenAsync();

            string sql = "use store; select * from products;";

            using (var command = new MySqlCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        byte[] blob = (byte[])reader["description"];
                        string description = System.Text.Encoding.UTF8.GetString(blob);


                        Product product = new Product
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Description = description,
                            Price = reader.GetDecimal("price"),
                            ImageURL = reader.GetString("imageURL"),
                            Category = category.GetCategoryById(reader.GetInt32("category"))
                        };

                        products.Add(product);
                    }
                }
            }
        }
        return products;
    }


    public async Task AddProductAsync(Product product, StoreLogic.ProductAddedHandler newProductDelegate = null)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            await connection.OpenAsync();

            string insertProductQuery = @"
                    use store;
                    INSERT INTO products (name, description, price, imageURL, category)
                    VALUES (@name, @description, @price, @imageURL, @category);";

            using (var insertCommand = new MySqlCommand(insertProductQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@name", product.Name);
                insertCommand.Parameters.AddWithValue("@description", product.Description);
                insertCommand.Parameters.AddWithValue("@price", product.Price);
                insertCommand.Parameters.AddWithValue("@imageURL", product.ImageURL);
                insertCommand.Parameters.AddWithValue("@category", product.Category.Id.ToString());
                await insertCommand.ExecuteNonQueryAsync();
            }
        }
        newProductDelegate(product);


    }

    public static async Task<int> AddMessageAsync(string content)
{
    using (var connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
    {
        await connection.OpenAsync();
        using (var transaction = await connection.BeginTransactionAsync())
        {
            try
            {
                // Llama al método pasando la conexión y transacción
                var existingId = await CheckIfMessageExists(content, connection, transaction);
                if (existingId != null)
                {
                    transaction.Commit();
                    return existingId.Value;
                }

                string query = "INSERT INTO messages (content) VALUES (@content); SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@content", content);
                    var messageId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    transaction.Commit();
                    return messageId;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

private static async Task<int?> CheckIfMessageExists(string content, MySqlConnection connection, MySqlTransaction transaction)
{
    string query = "use store; SELECT id FROM messages WHERE content = @content LIMIT 1;";
    using (var command = new MySqlCommand(query, connection))
    {
        command.Transaction = transaction;
        command.Parameters.AddWithValue("@content", content);
        var result = await command.ExecuteScalarAsync();
        if (result != DBNull.Value && result != null)
        {
            return Convert.ToInt32(result);
        }
        return null;
    }
}

    public static async Task<List<Dictionary<string, object>>> GetLastThreeMessagesAsync()
    {
        var messages = new List<Dictionary<string, object>>();
        using (var connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            await connection.OpenAsync();
            string query = "use store; SELECT id, content FROM messages ORDER BY created_at DESC LIMIT 3;";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var message = new Dictionary<string, object>
                            {
                                { "id", reader.GetInt32("id") },
                                { "content", reader.GetString("content") }
                            };
                        messages.Add(message);
                    }
                }
            }
        }
        messages.Reverse();
        return messages;
    }

    public static async Task<List<Dictionary<string, object>>> GetMessagesByContentAsync(string content)
    {
        var messages = new List<Dictionary<string, object>>();
        using (var connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            await connection.OpenAsync();
            string query = "use store; SELECT id, content FROM messages WHERE content = @content;";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@content", content);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var message = new Dictionary<string, object>
                            {
                                { "id", reader.GetInt32("id") },
                                { "content", reader.GetString("content") }
                            };
                        messages.Add(message);
                    }
                }
            }
        }
        return messages;
    }
}