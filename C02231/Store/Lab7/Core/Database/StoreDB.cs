using Core;
using MySqlConnector;
using StoreAPI.models;

namespace StoreAPI.Database;
public sealed class StoreDB
{
    public static void CreateMysql()
    {
        var products = new List<Product>{

        new Product
        {
            Name = "Cinder",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768889_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9500,
            Id = 1
        },

       new Product
        {
            Name = "Scarlet",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768896_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9500,
            Id = 2
        },

       new Product
        {
            Name = "Cress",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768902_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9500,
            Id = 3
        },

        new Product
        {
            Name = "Winter",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768926_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 4
        },

        new Product
        {
            Name = "Fairest",
            Author = "Marissa Meyer",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250774057_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 8700,
            Id = 5
        },

        new Product
        {
            Name = "La Sociedad de la Nieve",
            Author = "Pablo Vierci",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786070794162_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 12800,
            Id = 6
        },

        new Product
        {
            Name = "En Agosto nos vemos",
            Author = "Gabriel García Márquez",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073911290_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 14900,
            Id = 7
        },

        new Product
        {
            Name = "El estrecho sendero entre deseos",
            Author = "Patrick Rothfuss",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9789585457935_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 12800,
            Id = 8
        },

        new Product
        {
            Name = "Alas de Sangre",
            Author = "Rebecca Yarros",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788408279990_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 19800,
            Id = 9
        },

       new Product
        {
            Name = "Corona de Medianoche",
            Author = "Sarah J. Mass",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073143691_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 15800,
            Id = 10
        },

       new Product
        {
            Name = "Carta de Amor a los Muertos",
            Author = "Ava Dellaira",
            ImgUrl = "https://m.media-amazon.com/images/I/41IETN4YxGL._SY445_SX342_.jpg",
            Price = 8900,
            Id = 11
        },

        new Product
        {
            Name = "Alicia en el país de las Maravillas",
            Author = "Lewis Carrol",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788415618713_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 7900,
            Id = 0
        },
         new Product
        {
            Name = "Alicia en el país de las Maravillas",
            Author = "Lewis Carrol",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788415618713_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 7900,
            Id = 12
        },
       new Product
        {
            Name = "Crecent City 1 House Of Earth And Blood",
            Author = "Sarah J. Maas",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781635574043_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price =19800,
            Id = 13
        },
        new Product
        {
            Name = "Crescent City 2 House Of Sky And Breath",
            Author = "Sarah J. Maas",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781635574074_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 19800,
            Id = 14
        },
        new Product
        {
            Name = "Crescent City 3 House Of Flame And Shadow",
            Author = "Sarah J. Maas",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781635574104_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 19800,
            Id = 15
        },
        new Product
        {
            Name = "Harry Potter And The Sorcerers Stone",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878929_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9900,
            Id = 16
        },
        new Product
        {
            Name = "Harry Potter And The Chamber Of Secrets",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878936_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9900,
            Id = 17
        },
        new Product
        {
            Name = "Harry Potter And The Prisoner Of Azkaban",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878943_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 9900,
            Id = 18
        },
        new Product
        {
            Name = "Harry Potter And The Goblet Of Fire",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878950_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 19
        },
        new Product
        {
            Name = "Harry Potter And The Order Of The Phoenix",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878967_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 20
        },
         new Product
        {
            Name = "Harry Potter And The Half-Blood Prince",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878974_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 21
        },
         new Product
        {
            Name = "Harry Potter And The Deathly Hallows",
            Author = "J.K Rowling",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878981_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 12800,
            Id = 22
        },
         new Product
        {
            Name = "The Hunger Games",
            Author = "Suzzane Collins",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9780439023528_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 23
        },
        new Product
        {
            Name = "Catching Fire",
            Author = "Suzzane Collins",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9780545586177_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 24
        },
        new Product
        {
            Name = "Mockingjay",
            Author = "Suzzane Collins",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9780545663267_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 25
        },
        new Product
        {
            Name = "Ballad Of Songbirds And Snakes",
            Author = "Suzzane Collins",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781339016573_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 26
        },
          new Product
        {
            Name = "Kingkiller Chronicle 1 The Name Of The Wind",
            Author = "Patrick Rothfuss",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9780756404741_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 8700,
            Id = 27
        },
          new Product
        {
            Name = "Kingkiller 2 The Wise Mans Fear",
            Author = "Patrick Rothfuss",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9780756404734_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 28
        },
          new Product
        {
            Name = "Slow Regard Of Silent Things",
            Author = "Patrick Rothfuss",
            ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9780756411329_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            Price = 11900,
            Id = 29
        }
      };



        // string connectionString = "Server=localhost;Database=mysql;Port=3306;Uid=root;Pwd=123456;";
        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            connection.Open();


            // Create the products table if it does not exist  //;
            string createTableQuery = @"

                DROP DATABASE IF EXISTS store; 
                CREATE DATABASE store;
                use store;
                
                CREATE TABLE IF NOT EXISTS products (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(100),
                    author VARCHAR(100),
                    price DECIMAL(10, 2),
                    imgUrl VARCHAR(255)
                );
                
                CREATE TABLE IF NOT EXISTS sales (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    purchase_date DATETIME NOT NULL,
                    total DECIMAL(10, 2) NOT NULL,
                    payment_method ENUM('0', '1'),
                    purchase_number VARCHAR(50) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS saleLines (
                    sale_id INT,
                    product_id INT,
                    quantity int,
                    final_price DECIMAL(10, 2),
                    PRIMARY KEY (sale_id, product_id),
                    FOREIGN KEY (sale_id) REFERENCES sales(Id),
                    FOREIGN KEY (product_id) REFERENCES products(id)
                );   

                INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                VALUES 
                    ('2024-04-01 10:00:00', 5000, '1', 'ABD12345'), ('2024-04-02 09:00:00', 15000, '1', 'NEW12345'),
                    ('2024-04-03 09:00:00', 15200, '1', 'WER8352'), ('2024-04-03 10:30:00', 8200, '0', 'QXR9235'),
                    ('2024-04-04 10:30:00', 8400, '0', 'YUI9204'), ('2024-04-05 11:45:00', 9700, '1', 'ZPL4110'),
                    ('2024-04-06 11:45:00', 9900, '1', 'CDE4682'), ('2024-04-06 12:00:00', 11200, '0', 'WSG7589'),
                    ('2024-04-07 12:00:00', 11500, '0', 'BGT5726'), ('2024-04-08 14:15:00', 13500, '1', 'AIB6427'),
                    ('2024-04-09 14:15:00', 8700, '0', 'OUY8534'), ('2024-04-09 14:15:00', 13800, '1', 'UIO3014'),
                    ('2024-04-09 15:30:00', 8700, '0', 'NEW97531'), ('2024-04-10 11:30:00', 7520, '0', 'GML54321'),
                    ('2024-04-10 15:30:00', 8900, '0', 'NEW53148'), ('2024-04-10 15:30:00', 8900, '0', 'JKL7809'),
                    ('2024-04-10 16:45:00', 7800, '1', 'NEW86420'), ('2024-04-10 16:45:00', 7800, '1', 'XVD7065'),
                    ('2024-04-11 13:45:00', 10050, '1', 'GKS98765'), ('2024-04-11 13:45:00', 10050, '1', 'GKS98765'),
                    ('2024-04-12 09:15:00', 63000, '1', 'XYZ12345'), ('2024-04-12 16:45:00', 7500, '1', 'ASD6123'),
                    ('2024-04-12 18:00:00', 9500, '0', 'NEW75309'), ('2024-04-12 18:00:00', 9500, '0', 'IKJ2491'),
                    ('2024-04-13 18:00:00', 9300, '0', 'NHJ4298'), ('2024-04-13 19:15:00', 8600, '1', 'NEW64257'),
                    ('2024-04-14 19:15:00', 8600, '1', 'RTM1473'), ('2024-04-15 19:15:00', 9700, '1', 'ZXW3749'),
                    ('2024-04-15 20:30:00', 7300, '0', 'DCE6301'), ('2024-04-16 09:00:00', 14000, '1', 'NEW98765'),
                    ('2024-04-16 20:30:00', 7200, '0', 'NEW97531'),('2024-04-16 20:30:00', 7200, '0', 'QWE9573'),
                    ('2024-04-17 10:30:00', 8100, '0', 'LKJ8902'), ('2024-04-18 09:00:00', 15400, '1', 'NEW86420'),
                    ('2024-04-18 11:45:00', 9500, '1', 'QRS98765'),('2024-04-19 10:30:00', 8600, '0', 'NEW75309'),
                    ('2024-04-19 10:30:00', 8600, '0', 'UIO1394'), ('2024-04-19 14:15:00', 8500, '1', 'NEW13579'),
                    ('2024-04-20 12:00:00', 11400, '0', 'NEW97531'),('2024-04-21 11:45:00', 10000, '1', 'NEW64257'),
                    ('2024-04-22 12:00:00', 11800, '0', 'NEW53148'),('2024-04-22 14:15:00', 13700, '1', 'NEW86420'),
                    ('2024-04-23 15:30:00', 8800, '0', 'NEW75309'), ('2024-04-24 16:45:00', 7700, '1', 'NEW64257'),
                    ('2024-04-24 16:45:00', 7800, '1', 'NOP86420'), ('2024-04-25 18:00:00', 8900, '0', 'DEF75309'),
                    ('2024-04-26 18:00:00', 9400, '0', 'NEW53148'), ('2024-04-27 19:15:00', 9600, '1', 'NEW98765'),
                    ('2024-04-27 19:15:00', 9600, '1', 'ABC64257'), ('2024-04-27 20:30:00', 7300, '0', 'MNO53148'),
                    ('2024-04-28 20:30:00', 7200, '0', 'NEW24680');
                    ";

            //50 ventas, incluir detalle
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
                            INSERT INTO products (name, author, price, imgUrl)
                            VALUES (@name, @author, @price, @imgUrl);";

                        using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", product.Name);
                            insertCommand.Parameters.AddWithValue("@author", product.Author);
                            insertCommand.Parameters.AddWithValue("@price", product.Price);
                            insertCommand.Parameters.AddWithValue("@imgUrl", product.ImgUrl);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    string insertSaleLineQuery = @"
                    INSERT INTO saleLines VALUES
                                        (1, 2, 1, 8800),(2, 4, 1, 7520),(3, 6, 1, 10500),
                                        (3, 10, 1, 14500),(4, 5, 1, 16500),(5, 13, 1, 7500),
                                        (6, 8, 1, 20000),(6, 9, 1, 11900),(6, 15, 1, 9900),
                                        (7, 16, 1, 13500), (8, 1, 1, 16800),(9, 3, 1, 8500),
                                        (10, 12, 1, 12500), (11, 1, 1, 8700), (12, 17, 1, 7800),
                                        (13, 5, 1, 8400), (14, 10, 1, 11500), (15, 13, 1, 8700),
                                        (16, 8, 1, 11200), (17, 4, 1, 15400), (18, 9, 1, 13800),
                                        (19, 6, 1, 13500), (20, 11, 1, 9500), (21, 19, 1, 9700),
                                        (22, 16, 1, 8900), (23, 18, 1, 9300), (24, 15, 1, 9700),
                                        (25, 2, 1, 15200), (26, 3, 1, 9700), (27, 1, 1, 11200),
                                        (28, 7, 1, 9900), (29, 14, 1, 8600), (30, 12, 1, 11400),
                                        (31, 20, 1, 11800), (32, 21, 1, 13700), (33, 22, 1, 8800),
                                        (34, 23, 1, 9200), (35, 24, 1, 7700), (36, 25, 1, 7800),
                                        (37, 26, 1, 8900), (38, 27, 1, 9400), (39, 28, 1, 9600),
                                        (40, 29, 1, 9600), (41, 20, 1, 7300), (42, 3, 1, 7200),
                                        (43, 22, 1, 9000), (44, 13, 1, 8600), (45, 4, 1, 10000),
                                        (46, 25, 1, 11800), (47, 26, 1, 13500), (48, 27, 1, 9800),
                                        (49, 28, 1, 8500), (50, 9, 1, 13800), (50, 29, 1, 13800);                                        
                                        ";

                    using (var insertCommand = new MySqlCommand(insertSaleLineQuery, connection, transaction))
                    {

                        insertCommand.ExecuteNonQuery();
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
    internal static List<Dictionary<string, string>> RetrieveDatabaseInfo()
    {
        List<Dictionary<string, string>> databaseInfo = new List<Dictionary<string, string>>();


        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM products";

            using (var command = new MySqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> row = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = reader.GetValue(i).ToString();
                            row[columnName] = columnValue;
                        }
                        databaseInfo.Add(row);
                    }
                }
            }
        }

        return databaseInfo;
    }


    public async Task<IEnumerable<DaySalesReports>> GetDailySalesAsync(DateTime date)
    {
        if (date == DateTime.MinValue) throw new ArgumentException($"Invalid date provided: {nameof(date)}");

        List<DaySalesReports> dailySales = new List<DaySalesReports>();

        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            await connection.OpenAsync();

            string selectQuery = @"
            use store;
            SELECT S.purchase_number AS purchase_Number, S.purchase_date AS purchase_date, S.total AS total,  SUM(Sl.quantity) AS quantity, GROUP_CONCAT(P.name SEPARATOR ', ') AS productsName
            FROM sales S
            INNER JOIN saleLines Sl ON S.id = Sl.sale_id
			INNER JOIN products P ON Sl.product_id = P.id 
            WHERE DATE(S.purchase_date) = @date
            GROUP BY S.purchase_number, S.purchase_date, S.total";

            using (var command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@date", date);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        DateTime purchaseDate = reader.GetDateTime("purchase_date");
                        string purchaseNumber = reader.GetString("purchase_Number");
                        int quantity = reader.GetInt32("quantity");
                        decimal total = reader.GetDecimal("total");
                        string productsString = reader.GetString("productsName");
                        DaySalesReports dayReport = new DaySalesReports(purchaseDate, purchaseNumber.ToString(), quantity, total, productsString);
                        dailySales.Add(dayReport);
                    }
                }
            }
        }

        return dailySales;
    }

    public async Task<IEnumerable<WeekSalesReport>> GetWeeklySalesAsync(DateTime date)
    {
        if (date == DateTime.MinValue) throw new ArgumentException($"Invalid date provided: {nameof(date)}");

        List<WeekSalesReport> weeklySales = new List<WeekSalesReport>();

        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            await connection.OpenAsync();

            string selectQuery = @"
                use store;
                SELECT DAYNAME(S.purchase_date) AS day, SUM(S.total) AS total
                FROM sales S 
                WHERE YEARWEEK(S.purchase_date) = YEARWEEK(@date)
                GROUP BY DAYNAME(S.purchase_date);";

            using (var command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@date", date);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string? day = reader.GetString("day").ToString();
                        decimal total = reader.GetDecimal("total");
                        WeekSalesReport weekSalesReport = new WeekSalesReport(day, total);
                        weeklySales.Add(weekSalesReport);
                    }
                }
            }
        }

        return weeklySales;
    }

}
