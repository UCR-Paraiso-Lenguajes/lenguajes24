using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using TodoApi.Models;

namespace TodoApi.Database
{

    public sealed class StoreDB
    {

        public static void CreateMysql()
        {

            var products = Store.Instance.Products;

            string connectionString = "Server=localhost;Database=mysql;Port=3306;Uid=root;Pwd=123456;";
            using (var connection = new MySqlConnection(connectionString))
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
                );" ;

                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    int result = command.ExecuteNonQuery();
                    bool dbNoCreated = result < 0;
                    if (dbNoCreated)
                        throw new Exception("Error al crear la bd");
                }

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
                                insertCommand.Parameters.AddWithValue("@name", product.name);
                                insertCommand.Parameters.AddWithValue("@price", product.price);
                                insertCommand.ExecuteNonQuery();
                            }
                        }

                         string insertPaymentQuery = @"
                            INSERT INTO paymentMethods (paymentId, paymentName)
                            VALUES (@id, @name);";

                    using (var insertPaymentCommand = new MySqlCommand(insertPaymentQuery, connection, transaction))
                    {
                        insertPaymentCommand.Parameters.AddWithValue("@id", "0");
                        insertPaymentCommand.Parameters.AddWithValue("@name", "Efectivo");
                        insertPaymentCommand.ExecuteNonQuery();

                        using (var insertSinpeCommand = new MySqlCommand(insertPaymentQuery, connection, transaction))
                        {
                            insertSinpeCommand.Parameters.AddWithValue("@id", "1");
                            insertSinpeCommand.Parameters.AddWithValue("@name", "Sinpe");
                            insertSinpeCommand.ExecuteNonQuery();
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
    }
}