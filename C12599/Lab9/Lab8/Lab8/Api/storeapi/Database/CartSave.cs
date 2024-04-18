using MySqlConnector;
using System;
using System.Collections.Generic;

namespace storeapi
{
    public class CartSave
    {
        private readonly string _connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

        public void SaveSaleAndItemsToDatabase(Sale sale)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        EnsureItemsTableExists(connection, transaction);

                        foreach (var product in sale.Products)
                        {
                            InsertItem(connection, transaction, product, sale.PurchaseNumber, sale.Address);
                        }

                        EnsureComprasTableExists(connection, transaction);

                        InsertSale(connection, transaction, sale);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error al guardar en la base de datos: {ex.Message}");
                    }
                }
            }
        }

        private void EnsureItemsTableExists(MySqlConnection connection, MySqlTransaction transaction)
        {
            string createItemsTableQuery = @"
                CREATE TABLE IF NOT EXISTS Items (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    ProductId INT,
                    PurchaseNumber INT,
                    Address VARCHAR(255),
                    FOREIGN KEY (PurchaseNumber) REFERENCES Compras(purchaseNumber),
                    FOREIGN KEY (id) REFERENCES products(id)
                );
            ";

            using (var command = new MySqlCommand(createItemsTableQuery, connection, transaction))
            {
                command.ExecuteNonQuery();
            }
        }

     private void EnsureComprasTableExists(MySqlConnection connection, MySqlTransaction transaction)
{
    string createComprasTableQuery = @"
        CREATE TABLE IF NOT EXISTS Compras (
            total DECIMAL(10, 2) NOT NULL,
            date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            purchaseNumber INT NOT NULL,
            Paymethod INT,
            PRIMARY KEY (purchaseNumber),
            FOREIGN KEY (Paymethod) REFERENCES paymentMethods(id)
            
        );
    ";

    using (var command = new MySqlCommand(createComprasTableQuery, connection, transaction))
    {
        command.ExecuteNonQuery();
    }
}

        private void InsertItem(MySqlConnection connection, MySqlTransaction transaction, Product product, string purchaseNumber, string address)
        {
            string insertItemQuery = @"
                INSERT INTO Items (ProductId, PurchaseNumber, Address)
                VALUES (@ProductId, @PurchaseNumber, @Address);
            ";

            using (var command = new MySqlCommand(insertItemQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@ProductId", product.id);
                command.Parameters.AddWithValue("@PurchaseNumber", purchaseNumber);
                command.Parameters.AddWithValue("@Address", address);

                command.ExecuteNonQuery();
            }
        }

        private void InsertSale(MySqlConnection connection, MySqlTransaction transaction, Sale sale)
        {
            string insertSaleQuery = @"
                INSERT INTO Compras (total, date, purchaseNumber, Paymethod)
                VALUES (@total, @date, @purchaseNumber, @Paymethod);
            ";

            using (var command = new MySqlCommand(insertSaleQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@total", sale.Amount);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@purchaseNumber", sale.PurchaseNumber);
                command.Parameters.AddWithValue("@Paymethod", (int)sale.PaymentMethod);

                command.ExecuteNonQuery();
            }
        }
    }
}
