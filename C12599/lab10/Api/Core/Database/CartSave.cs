using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeapi.Models;
using core;

namespace storeapi.Database
{
    public class CartSave
    {
        
        

        public static async Task SaveSaleAndItemsToDatabaseAsync(Sale sale)
        {
            string saleValidationMessage = ValidateSaleForDatabase(sale);

            if (!string.IsNullOrWhiteSpace(saleValidationMessage))
            {
                throw new ArgumentException(saleValidationMessage);
            }

            await using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();

                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        await InsertSaleAsync(connection, transaction, sale);
                        await InsertManualComprasAsync(connection, transaction);
                        await InsertItemsAsync(connection, transaction, sale);

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw; // Propaga la excepción hacia arriba
                    }
                }
            }
        }

        private static string ValidateSaleForDatabase(Sale sale)
        {
            if (sale == null)
            {
                return "El objeto Sale no puede ser nulo.";
            }

            if (sale.Products == null || !sale.Products.Any())
            {
                return "El objeto Sale o la lista de productos no pueden ser nulos o vacíos.";
            }

            if (string.IsNullOrWhiteSpace(sale.PurchaseNumber) || string.IsNullOrWhiteSpace(sale.Address))
            {
                return "El número de compra (PurchaseNumber) y la dirección (Address) no pueden ser nulos o vacíos.";
            }

            return string.Empty;
        }

        private static async Task InsertSaleAsync(MySqlConnection connection, MySqlTransaction transaction, Sale sale)
        {
            ValidateSaleForInsert(sale);

            string insertSaleQuery = @"
                INSERT INTO Compras (total, date, purchaseNumber, Paymethod)
                VALUES (@total, @date, @purchaseNumber, @Paymethod);";

            using (var command = new MySqlCommand(insertSaleQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@total", sale.Amount);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@purchaseNumber", sale.PurchaseNumber);
                command.Parameters.AddWithValue("@Paymethod", (int)sale.PaymentMethod);

                await command.ExecuteNonQueryAsync();
            }
        }

        private static void ValidateSaleForInsert(Sale sale)
        {
            if (sale == null)
            {
                throw new ArgumentNullException(nameof(sale), "El objeto Sale no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(sale.PurchaseNumber))
            {
                throw new ArgumentException("El número de compra (PurchaseNumber) no puede ser nulo o vacío.", nameof(sale.PurchaseNumber));
            }
        }

        private static async Task InsertManualComprasAsync(MySqlConnection connection, MySqlTransaction transaction)
        {
            string insertManualCompras = @"
                -- Aquí se incluyen las inserciones manuales simuladas como operaciones asíncronas
                -- por ejemplo:
                -- INSERT INTO Compras (total, date, purchaseNumber, Paymethod) VALUES (...);
            ";

            using (var command = new MySqlCommand(insertManualCompras, connection, transaction))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        private static async Task InsertItemsAsync(MySqlConnection connection, MySqlTransaction transaction, Sale sale)
        {
            foreach (var product in sale.Products)
            {
                ValidateItemForInsert(product, sale.PurchaseNumber, sale.Address);

                string insertItemQuery = @"
                    INSERT INTO Items (ProductId, PurchaseNumber, Address, Price)
                    VALUES (@ProductId, @PurchaseNumber, @Address, (SELECT price FROM products WHERE id = @ProductId));";

                using (var command = new MySqlCommand(insertItemQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@ProductId", product.id);
                    command.Parameters.AddWithValue("@PurchaseNumber", sale.PurchaseNumber);
                    command.Parameters.AddWithValue("@Address", sale.Address);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private static void ValidateItemForInsert(Product product, string purchaseNumber, string address)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "El objeto Product no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(purchaseNumber))
            {
                throw new ArgumentException("El número de compra (PurchaseNumber) no puede ser nulo o vacío.", nameof(purchaseNumber));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("La dirección (Address) no puede ser nula o vacía.", nameof(address));
            }
        }

        public static async Task EnsureItemsTableExistsAsync()
        {
            string createItemsTableQuery = @"
                CREATE TABLE IF NOT EXISTS Items (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    ProductId INT,
                    PurchaseNumber VARCHAR(255),
                    Address VARCHAR(255),
                    Price DECIMAL(10, 2),
                    FOREIGN KEY (PurchaseNumber) REFERENCES Compras(purchaseNumber),
                    FOREIGN KEY (ProductId) REFERENCES products(id)
                );";

             await using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(createItemsTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task EnsureComprasTableExistsAsync()
        {
            string createComprasTableQuery = @"
                CREATE TABLE IF NOT EXISTS Compras (
                    total DECIMAL(10, 2) NOT NULL,
                    date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    purchaseNumber VARCHAR(255) NOT NULL,
                    Paymethod INT,
                    PRIMARY KEY (purchaseNumber)
                );";

            await using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(createComprasTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
