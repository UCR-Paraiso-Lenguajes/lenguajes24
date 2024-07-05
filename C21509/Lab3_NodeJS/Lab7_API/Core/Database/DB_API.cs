using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Store_API.Models;
using Core.Models;
using System.Diagnostics;

namespace Store_API.Database
{
    public class DB_API
    {
        private static string connectionString = "server=localhost;user=root;password=123456;database=Store_API";

        public DB_API(string connectionStrings)
        {
            connectionString = connectionStrings;
        }

        public DB_API() { }

        public void ConnectDB()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string createTablePaymentMethod = @"
                        CREATE TABLE IF NOT EXISTS PaymentMethod (
                           PaymentMethodId INT PRIMARY KEY,
                           PaymentMethodName VARCHAR(10) NOT NULL
                        );";

                    using (MySqlCommand command = new MySqlCommand(createTablePaymentMethod, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createTableSales = @"
                        CREATE TABLE IF NOT EXISTS Sales (
                            IdSale INT AUTO_INCREMENT PRIMARY KEY,                            
                            PurchaseNumber VARCHAR(50) NOT NULL,                           
                            Total DECIMAL(10, 2) NOT NULL,
                            Subtotal DECIMAL(10, 2) NOT NULL,                                                
                            Address VARCHAR(255) NOT NULL,
                            PaymentMethodId INT NOT NULL,
                            DateSale DATETIME NOT NULL,
                            FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethod(PaymentMethodId)
                        );";

                    using (MySqlCommand command = new MySqlCommand(createTableSales, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createTableProducts = @"
                        CREATE TABLE IF NOT EXISTS Products (
                            IdProduct INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(255) NOT NULL,
                            ImageURL VARCHAR(255),
                            Description TEXT,
                            Price DECIMAL(10, 2) NOT NULL,
                            Categoria INT NOT NULL
                        );";

                    using (MySqlCommand command = new MySqlCommand(createTableProducts, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createTableSalesLines = @"
                        CREATE TABLE IF NOT EXISTS SalesLines (
                            IdSaleLine INT AUTO_INCREMENT PRIMARY KEY,
                            IdSale INT NOT NULL,
                            IdProduct INT NOT NULL,
                            Quantity INT NOT NULL DEFAULT 1,
                            Price DECIMAL(10, 2) NOT NULL,
                            FOREIGN KEY (IdSale) REFERENCES Sales(IdSale),
                            FOREIGN KEY (IdProduct) REFERENCES Products(IdProduct)
                        );";

                    using (MySqlCommand command = new MySqlCommand(createTableSalesLines, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createTableNotification = @"
                        CREATE TABLE IF NOT EXISTS Notifications (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(255) NOT NULL,
                            Message TEXT NOT NULL,
                            Creation_Date DATETIME NOT NULL,
                            Status BIT NOT NULL DEFAULT 1
                        );";
                    using (MySqlCommand command = new MySqlCommand(createTableNotification, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void InsertProductsStore(List<Product> allProducts)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var actualProduct in allProducts)
                    {
                        string insertQuery = @"
                            INSERT INTO Products (Name, ImageURL, Description, Price, Categoria)
                            VALUES (@name, @imageURL, @description, @price, @categoria);
                        ";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@name", actualProduct.Name);
                            command.Parameters.AddWithValue("@imageURL", actualProduct.ImageURL);
                            command.Parameters.AddWithValue("@description", actualProduct.Description);
                            command.Parameters.AddWithValue("@price", actualProduct.Price);
                            command.Parameters.AddWithValue("@categoria", actualProduct.Categoria.IdCategory);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Product> SelectProducts()
        {
            List<Product> productListToStoreInstance = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectProducts = @"
                    SELECT IdProduct, Name, ImageURL, Description, Price, Categoria
                    FROM Products;
                ";

                using (MySqlCommand command = new MySqlCommand(selectProducts, connection))
                {
                    using (MySqlDataReader readerTable = command.ExecuteReader())
                    {
                        while (readerTable.Read())
                        {
                            int categoryId = Convert.ToInt32(readerTable["Categoria"]);
                            Category category = Categories.GetCategoryById(categoryId);

                            productListToStoreInstance.Add(new Product
                            {
                                Id = Convert.ToInt32(readerTable["IdProduct"]),
                                Name = readerTable["Name"].ToString(),
                                ImageURL = readerTable["ImageURL"].ToString(),
                                Description = readerTable["Description"].ToString(),
                                Price = Convert.ToDecimal(readerTable["Price"]),
                                Categoria = category
                            });
                        }
                    }
                }
            }

            return productListToStoreInstance;
        }

        public async Task<string> InsertSaleAsync(Sale sale, List<ProductQuantity> productQuantities)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        await InsertPaymentMethodsAsync(connection, transaction);

                        string insertSale = @"
                            INSERT INTO Sales (Total, Subtotal, PurchaseNumber, Address, PaymentMethodId, DateSale)
                            VALUES (@total, @subtotal, @purchaseNumber, @address, @paymentMethod, @dateSale);
                        ";

                        using (MySqlCommand command = new MySqlCommand(insertSale, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@total", sale.Amount);
                            command.Parameters.AddWithValue("@subtotal", sale.Amount);
                            command.Parameters.AddWithValue("@purchaseNumber", sale.PurchaseNumber);
                            command.Parameters.AddWithValue("@address", sale.Address);
                            command.Parameters.AddWithValue("@paymentMethod", (int)sale.PaymentMethod);
                            command.Parameters.AddWithValue("@dateSale", DateTime.Now);
                            await command.ExecuteNonQueryAsync();
                        }

                        await InsertSalesLinesAsync(connection, transaction, sale.PurchaseNumber, productQuantities);

                        await transaction.CommitAsync();

                        return sale.PurchaseNumber;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        private async Task InsertPaymentMethodsAsync(MySqlConnection connection, MySqlTransaction transaction)
        {
            string insertPaymentMethodQuery = @"
                INSERT INTO PaymentMethod (PaymentMethodId, PaymentMethodName)
                VALUES (@idPayment, @paymentName)
                ON DUPLICATE KEY UPDATE PaymentMethodName = VALUES(PaymentMethodName);
            ";

            var paymentMethods = new List<(int id, string name)>
            {
                (0, "Efectivo"),
                (1, "Sinpe")
            };

            using (MySqlCommand command = new MySqlCommand(insertPaymentMethodQuery, connection, transaction))
            {
                foreach (var paymentMethod in paymentMethods)
                {
                    command.Parameters.AddWithValue("@idPayment", paymentMethod.id);
                    command.Parameters.AddWithValue("@paymentName", paymentMethod.name);
                    await command.ExecuteNonQueryAsync();
                    command.Parameters.Clear();
                }
            }
        }

        private async Task InsertSalesLinesAsync(MySqlConnection connection, MySqlTransaction transaction, string purchaseNumber, List<ProductQuantity> products)
        {
            string selectIdSale = "SELECT IdSale FROM Sales WHERE PurchaseNumber = @purchaseNumber";
            int idSaleFromSelect;

            using (MySqlCommand command = new MySqlCommand(selectIdSale, connection, transaction))
            {
                command.Parameters.AddWithValue("@purchaseNumber", purchaseNumber);
                idSaleFromSelect = Convert.ToInt32(await command.ExecuteScalarAsync());
            }

            string insertSalesLine = @"
                INSERT INTO SalesLines (IdSale, IdProduct, Price, Quantity)
                VALUES (@idSale, @idProduct, @price, @quantity);
            ";

            foreach (var product in products)
            {
                using (MySqlCommand command = new MySqlCommand(insertSalesLine, connection, transaction))
                {
                    command.Parameters.AddWithValue("@idSale", idSaleFromSelect);
                    command.Parameters.AddWithValue("@idProduct", product.Id);
                    command.Parameters.AddWithValue("@price", product.Price);
                    command.Parameters.AddWithValue("@quantity", product.Quantity);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<SaleAttribute>> ObtainDailySalesAsync(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                throw new ArgumentException("The date cannot be", nameof(date));
            }

            var salesReport = new List<SaleAttribute>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT s.IdSale, s.PurchaseNumber, s.Total, s.DateSale, p.Name AS Product
                    FROM Sales s
                    JOIN SalesLines sl ON s.IdSale = sl.IdSale
                    JOIN Products p ON sl.IdProduct = p.IdProduct
                    WHERE DATE(s.DateSale) = @date;
                ";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", date.Date);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var sale = new SaleAttribute
                            {
                                SaleId = reader.GetInt32(0),
                                PurchaseNumber = reader.GetString(1),
                                Total = reader.GetDecimal(2),
                                PurchaseDate = reader.GetDateTime(3),
                                Product = reader.GetString(4),
                                DailySale = date.ToString("dddd"),
                                SaleCounter = 1
                            };

                            salesReport.Add(sale);
                        }
                    }
                }
            }

            return salesReport;
        }

        public async Task<IEnumerable<SaleAttribute>> ObtainWeeklySalesAsync(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                throw new ArgumentException("The date cannot be DateTime.MinValue.", nameof(date));
            }

            var weeklySalesReport = new List<SaleAttribute>();

            var startOfWeek = date.AddDays(-(int)date.DayOfWeek);

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT DAYNAME(s.DateSale) AS SaleDayOfWeek, SUM(s.Total) AS TotalSales
                    FROM Sales s
                    WHERE YEARWEEK(s.DateSale) = YEARWEEK(@startDate)
                    GROUP BY SaleDayOfWeek
                    ORDER BY SaleDayOfWeek;
                ";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startOfWeek);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var saleDayOfWeek = reader.GetString(0);
                            var totalSales = reader.GetDecimal(1);

                            var salesByDay = new SaleAttribute
                            {
                                DailySale = saleDayOfWeek,
                                Total = totalSales
                            };

                            weeklySalesReport.Add(salesByDay);
                        }
                    }
                }
            }

            return weeklySalesReport;
        }

        public async Task<Product> InsertionProductInDBAsync(Product insertedProduct)
        {
            if (insertedProduct == null) throw new ArgumentException($"{nameof(insertedProduct)} cannot be null");

            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try
            {
                connectionWithDB = new MySqlConnection(connectionString);
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string insertQuery = @"
                    INSERT INTO Products (Name, ImageUrl, Description, Price, Categoria)
                    VALUES (@name, @imageUrl, @description, @price, @categoria);
                    SELECT LAST_INSERT_ID();
                ";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connectionWithDB))
                {
                    command.Transaction = transaction;

                    command.Parameters.AddWithValue("@name", insertedProduct.Name);
                    command.Parameters.AddWithValue("@imageUrl", insertedProduct.ImageURL);
                    command.Parameters.AddWithValue("@description", insertedProduct.Description);
                    command.Parameters.AddWithValue("@price", insertedProduct.Price);
                    command.Parameters.AddWithValue("@categoria", insertedProduct.Categoria.IdCategory);

                    var result = await command.ExecuteScalarAsync();
                    insertedProduct.Id = Convert.ToInt32(result);
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (connectionWithDB != null)
                {
                    await connectionWithDB.CloseAsync();
                }
            }

            return insertedProduct;
        }

        public async Task<bool> DeleteProductFromDBAsync(int productId)
        {
            if (productId <= 0) throw new ArgumentException($"{nameof(productId)} must be greater than zero");

            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try
            {
                connectionWithDB = new MySqlConnection(connectionString);
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string deleteQuery = @"
                    DELETE FROM Products
                    WHERE Id = @productId;
                ";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, connectionWithDB))
                {
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@productId", productId);
                    await command.ExecuteNonQueryAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (connectionWithDB != null)
                {
                    await connectionWithDB.CloseAsync();
                }
            }

            return true;
        }

        public async Task<IEnumerable<Notifications>> getNotificationsForAdminsAsync()
        {
            List<Notifications> listOfNotifications = new List<Notifications>();
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try
            {
                connectionWithDB = new MySqlConnection(connectionString);
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string selectNotifications = @"
                    SELECT Id, Name, Message, Creation_Date, Status
                    FROM Notifications
                    ORDER BY Status DESC, Creation_Date DESC;
                ";

                using (MySqlCommand command = new MySqlCommand(selectNotifications, connectionWithDB))
                {
                    command.Transaction = transaction;
                    using (var readerTable = await command.ExecuteReaderAsync())
                    {
                        while (await readerTable.ReadAsync())
                        {
                            var id = Convert.ToInt32(readerTable["Id"]);
                            var name = readerTable["Name"].ToString();
                            var message = readerTable["Message"].ToString();
                            var creationDate = (DateTime)readerTable["Creation_Date"];
                            var status = Convert.ToInt32(readerTable["Status"]);
                            var currentNotify = new Notifications(id, name, message, creationDate, status);
                            listOfNotifications.Add(currentNotify);
                        }
                    }
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await connectionWithDB.CloseAsync();
            }
            return listOfNotifications;
        }

        public async Task<IEnumerable<Notifications>> getNotificationsForUsersAsync()
        {
            List<Notifications> listOfNotifications = new List<Notifications>();
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try
            {
                connectionWithDB = new MySqlConnection(connectionString);
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string selectNotifications = @"
                    SELECT Id, Name, Message, Creation_Date, Status
                    FROM Notifications
                    WHERE Status <> 0
                    ORDER BY Creation_Date DESC
                    LIMIT 3;
                ";

                using (MySqlCommand command = new MySqlCommand(selectNotifications, connectionWithDB))
                {
                    command.Transaction = transaction;
                    using (var readerTable = await command.ExecuteReaderAsync())
                    {
                        while (await readerTable.ReadAsync())
                        {
                            var id = Convert.ToInt32(readerTable["Id"]);
                            var name = readerTable["Name"].ToString();
                            var message = readerTable["Message"].ToString();
                            var creationDate = (DateTime)readerTable["Creation_Date"];
                            var status = Convert.ToInt32(readerTable["Status"]);
                            var currentNotify = new Notifications(id, name, message, creationDate, status);
                            listOfNotifications.Add(currentNotify);
                        }
                    }
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await connectionWithDB.CloseAsync();
            }
            return listOfNotifications;
        }

        public async Task DeleteOneNotificationAsync(int notifyToDelete)
        {
            if (notifyToDelete <= 0) throw new ArgumentException($"{nameof(notifyToDelete)} cannot be null");
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try
            {
                connectionWithDB = new MySqlConnection(connectionString);
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string updateNotification = @"
                    UPDATE Notifications
                    SET Status = 0
                    WHERE Id = @notificationId;
                ";

                using (MySqlCommand command = new MySqlCommand(updateNotification, connectionWithDB))
                {
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@notificationId", notifyToDelete);
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("A delete error has ocurred", ex);
            }
            finally
            {
                await connectionWithDB.CloseAsync();
            }
        }

        public async Task<Notifications> InsertNotificationAsync(Notifications newNotification)
        {
            if (newNotification == null) throw new ArgumentException($"{nameof(newNotification)} cannot be null");

            Notifications insertedNotification = null;
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try
            {
                connectionWithDB = new MySqlConnection(connectionString);
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string insertNotification = @"
            INSERT INTO Notifications (Name, Message, Creation_Date, Status)
            VALUES (@name, @message, @creationDate, @status);
        ";
                using (MySqlCommand command = new MySqlCommand(insertNotification, connectionWithDB))
                {
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@name", newNotification.notificationName);
                    command.Parameters.AddWithValue("@message", newNotification.notificationMessage);
                    command.Parameters.AddWithValue("@creationDate", newNotification.notificationCreatedDate);
                    command.Parameters.AddWithValue("@status", newNotification.notificationStatus);
                    await command.ExecuteNonQueryAsync();

                    int thisIdNotification = 0;
                    string selectLastInsertedId = "SELECT LAST_INSERT_ID();";
                    using (MySqlCommand idCommand = new MySqlCommand(selectLastInsertedId, connectionWithDB))
                    {
                        idCommand.Transaction = transaction;
                        object result = await idCommand.ExecuteScalarAsync();
                        thisIdNotification = Convert.ToInt32(result);

                        insertedNotification = new Notifications(thisIdNotification, newNotification.notificationName, newNotification.notificationMessage, newNotification.notificationCreatedDate, 1);
                    }
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An insertion error has occurred", ex);
            }
            finally
            {
                await connectionWithDB.CloseAsync();
            }
            return insertedNotification;
        }
    }
}

