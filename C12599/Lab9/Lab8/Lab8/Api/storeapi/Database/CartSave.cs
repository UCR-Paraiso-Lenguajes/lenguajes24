using MySqlConnector;
using System;

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

                         InsertSale(connection, transaction, sale);

                        foreach (var product in sale.Products)
                        {
                            InsertItem(connection, transaction, product, sale.PurchaseNumber, sale.Address);
                        }

                       
                       

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

        public static void EnsureItemsTableExists()
        {
            string _connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string createItemsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Items (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        ProductId INT,
                        PurchaseNumber VARCHAR(255),
                        Address VARCHAR(255),
                        Price DECIMAL(10, 2),
                        FOREIGN KEY (PurchaseNumber) REFERENCES Compras(purchaseNumber),
                        FOREIGN KEY (ProductId) REFERENCES products(id)
                    )";

                using (var command = new MySqlCommand(createItemsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void EnsureComprasTableExists()
        {
            string _connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string createComprasTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Compras (
                        total DECIMAL(10, 2) NOT NULL,
                        date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        purchaseNumber VARCHAR(255) NOT NULL,
                        Paymethod INT,
                        PRIMARY KEY (purchaseNumber)
                        FOREIGN KEY (Paymethod) REFERENCES paymentMethods(id)
                        
                    )";

                using (var command = new MySqlCommand(createComprasTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

      private void InsertItem(MySqlConnection connection, MySqlTransaction transaction, Product product, string purchaseNumber, string address)
{
    string insertItemQuery = @"
        INSERT INTO Items (ProductId, PurchaseNumber, Address, Price)
        VALUES (@ProductId, @PurchaseNumber, @Address, (SELECT price FROM products WHERE id = @ProductId));
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
                VALUES (@total, @date, @purchaseNumber, @Paymethod)";

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


