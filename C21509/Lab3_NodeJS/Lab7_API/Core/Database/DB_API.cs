using MySql.Data.MySqlClient;
using Store_API.Models;

namespace Store_API.Database
{
    public class DB_API
    {
        private readonly string connectionString = "server=localhost;user=root;password=123456;database=Store_API";

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
                            Price DECIMAL(10, 2) NOT NULL
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
                            Price DECIMAL(10, 2) NOT NULL,
                            FOREIGN KEY (IdSale) REFERENCES Sales(IdSale),
                            FOREIGN KEY (IdProduct) REFERENCES Products(IdProduct)
                        );";

                    using (MySqlCommand command = new MySqlCommand(createTableSalesLines, connection))
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
                            INSERT INTO Products (Name, ImageURL, Price)
                            VALUES (@name, @imageURL, @price);
                        ";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@name", actualProduct.Name);
                            command.Parameters.AddWithValue("@imageURL", actualProduct.ImageURL);
                            command.Parameters.AddWithValue("@price", actualProduct.Price);

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

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string selectProducts = @"
                        SELECT IdProduct, Name, ImageURL, Price
                        FROM Products;
                        ";

                    using (MySqlCommand command = new MySqlCommand(selectProducts, connection))
                    {
                        using (MySqlDataReader readerTable = command.ExecuteReader())
                        {
                            while (readerTable.Read())
                            {
                                productListToStoreInstance.Add(new Product
                                {
                                    Id = Convert.ToInt32(readerTable["IdProduct"]),
                                    Name = readerTable["Name"].ToString(),
                                    ImageURL = readerTable["ImageURL"].ToString(),
                                    Price = Convert.ToDecimal(readerTable["Price"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return productListToStoreInstance;
        }

        public async Task<string> InsertSaleAsync(Sale sale)
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

                        await InsertSalesLinesAsync(connection, transaction, sale.PurchaseNumber, sale.Products.ToList());

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

        private async Task InsertSalesLinesAsync(MySqlConnection connection, MySqlTransaction transaction, string purchaseNumber, List<Product> products)
        {
            string selectIdSale = "SELECT IdSale FROM Sales WHERE PurchaseNumber = @purchaseNumber";
            decimal idSaleFromSelect;

            using (MySqlCommand command = new MySqlCommand(selectIdSale, connection, transaction))
            {
                command.Parameters.AddWithValue("@purchaseNumber", purchaseNumber);
                idSaleFromSelect = Convert.ToDecimal(await command.ExecuteScalarAsync());
            }

            string insertSalesLine = @"
        INSERT INTO SalesLines (IdSale, IdProduct, Price)
        VALUES (@idSale, @idProduct, @price);
    ";

            foreach (var product in products)
            {
                using (MySqlCommand command = new MySqlCommand(insertSalesLine, connection, transaction))
                {
                    command.Parameters.AddWithValue("@idSale", idSaleFromSelect);
                    command.Parameters.AddWithValue("@idProduct", product.Id);
                    command.Parameters.AddWithValue("@price", product.Price);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public void InsertSalesData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertSalesQuery = @"
                INSERT INTO Sales (PurchaseDate, Total, PaymentMethodId, PurchaseNumber)
                VALUES
                    ('2024-04-27 08:00:00', 50.00, 1, 'ACD789'),
                    ('2024-04-27 12:30:00', 35.75, 0, 'BTR321'),
                    ('2024-04-28 10:15:00', 75.20, 1, 'CJR579'),
                    ('2024-04-28 14:45:00', 20.50, 0, 'DET468'),
                    ('2024-04-29 09:20:00', 45.60, 0, 'EBY321'),
                    ('2024-04-29 16:00:00', 90.00, 1, 'FKJ789'),
                    ('2024-04-30 11:45:00', 60.30, 0, 'GNM579'),
                    ('2024-04-30 13:20:00', 25.75, 1, 'HFK468'),
                    ('2024-05-01 08:30:00', 55.00, 0, 'IMH321'),
                    ('2024-05-01 15:10:00', 70.25, 1, 'JLO789');
            ";

                    using (MySqlCommand command = new MySqlCommand(insertSalesQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string insertSalesLinesQuery = @"
                INSERT INTO SalesLines (IdSale, IdProduct, Price)
                VALUES
                    (1, 1, 50.00),
                    (2, 2, 35.75),
                    (3, 3, 75.20),
                    (4, 4, 20.50),
                    (5, 5, 45.60),
                    (6, 6, 90.00),
                    (7, 7, 60.30),
                    (8, 8, 25.75),
                    (9, 9, 55.00),
                    (10, 10, 70.25);
            ";

                    using (MySqlCommand command = new MySqlCommand(insertSalesLinesQuery, connection))
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
                throw new ArgumentException("The date cannot be", nameof(date));
            }

            var weeklySalesReport = new List<SaleAttribute>();
            var startOfWeek = date.AddDays(-(int)date.DayOfWeek);

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT DAYNAME(s.DateSale) AS SaleDayOfWeek, COUNT(*) AS SaleCount
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
                            var saleCount = reader.GetInt32(1);

                            var salesByDay = new SaleAttribute
                            {
                                DailySale = saleDayOfWeek,
                                SaleCounter = saleCount
                            };

                            weeklySalesReport.Add(salesByDay);
                        }
                    }
                }
            }

            return weeklySalesReport;
        }
    }
}