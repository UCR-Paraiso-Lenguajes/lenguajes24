using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using Core;
using System.Data;


namespace storeApi.db;
public sealed class StoreDB
{



    public StoreDB()
    {


    }

    public static void CreateMysql()
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
                Category = ProductCategory.Audifonos
            },
            new Product
            {
                Id = 2,
                Name = "Producto 2",
                Description = "Control PS4",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Controller2.png",
                Category = ProductCategory.Controles
            },
            new Product
            {
                Id = 3,
                Name = "Producto 3",
                Description = "PS4 1TB",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Playstation3.jpg",
                Category = ProductCategory.Consolas
            },
            new Product
            {
                Id = 4,
                Name = "Producto 4",
                Description = "Crash Bandicoot 4 Switch",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Game.png",
                Category = ProductCategory.Videojuegos
            },
            new Product
            {
                Id = 5,
                Name = "Producto 5",
                Description = "Mouse Logitech",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_Quad_Mouse.jpg",
                Category = ProductCategory.Mouse
            },
            new Product
            {
                Id = 6,
                Name = "Producto 6",
                Description = "Silla Oficina",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_Quad_Chair.jpg",
                Category = ProductCategory.Sillas
            },
            new Product
            {
                Id = 7,
                Name = "Producto 7",
                Description = "Laptop Acer",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Laptop.png",
                Category = ProductCategory.Laptops
            },
            new Product
            {
                Id = 8,
                Name = "Producto 8",
                Description = "Oculus Quest 3",
                Price = 20000,
                ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Oculus2.jpg",
                Category = ProductCategory.RealidadVirtual
            },
            new Product
            {
                Id = 9,
                Name = "Producto 9",
                Description = "Teclado mecánico RGB",
                Price = 15000,
                ImageURL = "https://m.media-amazon.com/images/I/61uofDvRldS._AC_UL320_.jpg",
                Category = ProductCategory.Teclados
            },
            new Product
            {
                Id = 10,
                Name = "Producto 10",
                Description = "Monitor gaming 144Hz",
                Price = 30000,
                ImageURL = "https://m.media-amazon.com/images/I/71sPOWyMwVL._AC_UL320_.jpg",
                Category = ProductCategory.Monitores
            },
            new Product
            {
                Id = 11,
                Name = "Producto 11",
                Description = "Cámara DSLR Canon EOS",
                Price = 40000,
                ImageURL = "https://m.media-amazon.com/images/I/61o0MBO9jFL._AC_UL320_.jpg",
                Category = ProductCategory.Camaras
            },
            new Product
            {
                Id = 12,
                Name = "Producto 12",
                Description = "Smartwatch Samsung Galaxy",
                Price = 25000,
                ImageURL = "https://m.media-amazon.com/images/I/711f6KLsMaL._AC_UL320_.jpg",
                Category = ProductCategory.Smartwatches
            },
            new Product
            {
                Id = 13,
                Name = "Producto 13",
                Description = "Bicicleta de montaña",
                Price = 150000,
                ImageURL = "https://m.media-amazon.com/images/I/817X9TvYQ3L._AC_UL320_.jpg",
                Category = ProductCategory.Bicicletas
            },
            new Product
            {
                Id = 14,
                Name = "Producto 14",
                Description = "Robot aspirador",
                Price = 35000,
                ImageURL = "https://m.media-amazon.com/images/I/619TvTYML3L._AC_UY218_.jpg",
                Category = ProductCategory.RobotsAspiradores
            },
            new Product
            {
                Id = 15,
                Name = "Producto 15",
                Description = "Proyector de cine en casa",
                Price = 50000,
                ImageURL = "https://m.media-amazon.com/images/I/71iPl3A0ubL._AC_UL320_.jpg",
                Category = ProductCategory.Proyectores
            },
            new Product
            {
                Id = 16,
                Name = "Producto 16",
                Description = "Cafetera espresso",
                Price = 20000,
                ImageURL = "https://m.media-amazon.com/images/I/71BvCt6eAFL._AC_UL320_.jpg",
                Category = ProductCategory.Cafeteras
            }
            };




        using (var connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            connection.Open();

            string createTableQuery = @"
                DROP DATABASE IF EXISTS store;
                CREATE DATABASE store;
                use store;

                CREATE TABLE IF NOT EXISTS paymentMethods (
                    paymentId INT PRIMARY KEY,
                    paymentName VARCHAR(30) NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS products (
                id INT AUTO_INCREMENT PRIMARY KEY,
                name VARCHAR(100),
                description TEXT,
                price DECIMAL(10, 2),
                imageURL VARCHAR(255),
                category VARCHAR(30)
                );

                
                CREATE TABLE IF NOT EXISTS sales (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    purchase_date DATETIME NOT NULL,
                    total DECIMAL(10, 2) NOT NULL,
                    payment_method INT NOT NULL,
                    purchase_number VARCHAR(50) NOT NULL,
                    INDEX idx_purchase_number (purchase_number), 
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
                
                INSERT INTO paymentMethods (paymentId, paymentName)
                VALUES 
                    (0, 'Cash'),
                    (1, 'Sinpe');
                    
                INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                VALUES
                    ('2024-05-10 05:20:00', 67.20, 1, 'SA123456789'),
                    ('2024-05-11 14:45:00', 45.80, 0, 'SB987654321'),
                    ('2024-05-12 08:55:00', 35.60, 1, 'SC246813579'),
                    ('2024-05-13 17:30:00', 78.90, 0, 'SD135792468'),
                    ('2024-05-14 10:10:00', 25.50, 0, 'SE987654321'),
                    ('2024-05-15 12:25:00', 50.30, 1, 'SF123456789'),
                    ('2024-05-16 13:15:00', 90.00, 0, 'SG246813579'),
                    ('2024-05-17 07:50:00', 55.25, 1, 'SH135792468'),
                    ('2024-05-18 11:40:00', 40.75, 0, 'SI987654321'),
                    ('2024-05-19 09:05:00', 70.00, 1, 'SJ123456789'),
                    ('2024-05-20 15:20:00', 60.40, 1, 'SX123456789'),
                    ('2024-05-21 08:55:00', 35.75, 0, 'SY987654321'),
                    ('2024-05-22 12:30:00', 80.20, 1, 'SZ246813579'),
                    ('2024-05-23 09:45:00', 25.50, 0, 'SW135792468'),
                    ('2024-05-24 11:00:00', 45.90, 0, 'SV987654321'),
                    ('2024-05-25 14:10:00', 70.80, 1, 'SU123456789'),
                    ('2024-05-26 16:25:00', 50.70, 0, 'ST246813579'),
                    ('2024-05-27 10:35:00', 35.25, 1, 'SS135792468'),
                    ('2024-05-28 13:20:00', 67.00, 0, 'SR987654321'),
                    ('2024-05-29 07:15:00', 40.25, 1, 'SQ123456789'),
                    ('2024-05-01 08:14:00', 90.25, 0, 'SG123456459'),
                    ('2024-05-04 08:23:00', 92.25, 1, 'HG123494459'),
                    ('2024-05-04 08:09:00', 93.25, 0, 'SV123475459'),
                    ('2024-05-04 08:18:00', 95.25, 1, 'TE123434459'),
                    ('2024-05-10 05:20:00', 67.20, 1, 'SA124456789'),
                    ('2024-05-10 05:20:00', 67.20, 1, 'SA122456789'),
                    ('2024-05-10 05:20:00', 67.20, 1, 'SA121456789'),
                    ('2024-05-10 05:20:00', 67.20, 1, 'SA129456789')

                                ";


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
                            INSERT INTO products (name, description, price, imageURL, category)
                            VALUES (@name, @description ,@price, @imageURL, @category);";

                        using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", product.Name);
                            insertCommand.Parameters.AddWithValue("@description", product.Description);
                            insertCommand.Parameters.AddWithValue("@price", product.Price);
                            insertCommand.Parameters.AddWithValue("@imageURL", product.ImageURL);
                            insertCommand.Parameters.AddWithValue("@category", product.Category.ToString());
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
    public static List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionDB.Instance.ConnectionString))
        {
            connection.Open();

            string sql = "use store; select * from products;";

            using (var command = new MySqlCommand(sql, connection))
            {
                
                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Description = reader.GetString("description"),
                            Price = reader.GetDecimal("price"),
                            ImageURL = reader.GetString("imageURL"),
                            Category = Enum.Parse<ProductCategory>(reader.GetString("category"))
                        };

                        products.Add(product);
                    }
                }
            }
        }
        return products;
    }



}