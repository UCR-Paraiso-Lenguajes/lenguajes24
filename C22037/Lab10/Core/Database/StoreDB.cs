using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using TodoApi.Models;

namespace TodoApi.Database;
public sealed class StoreDB
{
    public static void CreateMysql()
    {
        var products = new List<Product>
            {
                new Product("Producto 1", "https://images-na.ssl-images-amazon.com/images/I/71JSM9i1bQL.AC_UL160_SR160,160.jpg", 10, "Descripción 1", 1),
                new Product("Producto 2", "https://images-na.ssl-images-amazon.com/images/I/418UoVylqyL._AC_UL160_SR160,160_.jpg", 20, "Descripción 2", 2),
                new Product("Producto 3", "https://images-na.ssl-images-amazon.com/images/I/81WsSyAYxHL._AC_UL160_SR160,160_.jpg", 30, "Descripción 3", 3),
                new Product("Producto 4", "https://images-na.ssl-images-amazon.com/images/I/51-lOBlIrFL._AC_UL160_SR160,160_.jpg", 40, "Descripción 4", 4),
                new Product("Producto 5", "https://images-na.ssl-images-amazon.com/images/I/51wD-xrtyWL._AC_UL160_SR160,160_.jpg", 50, "Descripción 5", 5),
                new Product("Producto 6", "https://images-na.ssl-images-amazon.com/images/I/71EZAE6fljL._AC_UL160_SR160,160_.jpg", 60, "Descripción 6", 6),
                new Product("Producto 7", "https://m.media-amazon.com/images/I/817EyM89DtL._AC_SY100_.jpg", 70, "Descripción 7", 7),
                new Product("Producto 8", "https://m.media-amazon.com/images/I/61J0e7d0GEL._AC_SY100_.jpg", 80, "Descripción 8", 8),
                new Product("Producto 9", "https://m.media-amazon.com/images/I/81mzvAGkHkL._AC_SY100_.jpg", 90, "Descripción 9", 9),
                new Product("Producto 10", "https://m.media-amazon.com/images/I/51YlAYwPx6L._AC_SY100_.jpg", 100, "Descripción 10", 10),
                new Product("Producto 11", "https://m.media-amazon.com/images/I/71cj5cNm7ZL._AC_UY218_.jpg", 110, "Descripción 11", 11),
                new Product("Producto 12", "https://m.media-amazon.com/images/I/7148mbvrbWL._AC_UL320_.jpg", 120, "Descripción 12", 12),
                new Product("Producto 13", "https://m.media-amazon.com/images/I/71Pf0aGicBL._AC_UY218_.jpg", 130, "Descripción 13", 13),
                new Product("Producto 14", "https://m.media-amazon.com/images/I/71P84KYUfrL._AC_UL320_.jpg", 140, "Descripción 14", 14),
                new Product("Producto 15", "https://m.media-amazon.com/images/I/51gJxciP-qL._AC_UY218_T2F_.jpg", 150, "Descripción 15", 15),
                new Product("Producto 16", "https://m.media-amazon.com/images/I/61OI1MNjZZL._AC_UY218_T2F_.jpg", 160, "Descripción 16", 16)            };

        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            connection.Open();

            // Create the products table if it does not exist
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
                    price DECIMAL(10, 2)
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
                    ('2024-04-27 08:00:00', 50.00, 1, 'A123456789'),
                    ('2024-04-27 12:30:00', 35.75, 0, 'B987654321'),
                    ('2024-04-28 10:15:00', 75.20, 1, 'C246813579'),
                    ('2024-04-28 14:45:00', 20.50, 0, 'D135792468'),
                    ('2024-04-29 09:20:00', 45.60, 0, 'E987654321'),
                    ('2024-04-29 16:00:00', 90.00, 1, 'F123456789'),
                    ('2024-04-30 11:45:00', 60.30, 0, 'G246813579'),
                    ('2024-04-30 13:20:00', 25.75, 1, 'H135792468'),
                    ('2024-05-01 08:30:00', 55.00, 0, 'I987654321'),
                    ('2024-05-01 15:10:00', 70.25, 1, 'J123456789');

                INSERT INTO sales (purchase_date, total, payment_method, purchase_number) 
                VALUES
                    ('2024-04-18 09:00:00', 40.00, 1, 'X123456789'),
                    ('2024-04-18 14:30:00', 55.25, 0, 'Y987654321'),
                    ('2024-04-19 11:45:00', 70.80, 1, 'Z246813579'),
                    ('2024-04-19 16:15:00', 30.50, 0, 'W135792468'),
                    ('2024-04-20 08:20:00', 65.40, 0, 'V987654321'),
                    ('2024-04-20 12:00:00', 80.00, 1, 'U123456789'),
                    ('2024-04-21 10:30:00', 50.70, 0, 'T246813579'),
                    ('2024-04-21 13:45:00', 35.25, 1, 'S135792468'),
                    ('2024-04-22 09:15:00', 60.00, 0, 'R987654321'),
                    ('2024-04-22 15:40:00', 75.25, 1, 'Q123456789');";


            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                int result = command.ExecuteNonQuery();
                bool dbNoCreated = result < 0;
                if (dbNoCreated) throw new Exception("Error creating the bd");
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