using Core;
using MySqlConnector;
using StoreAPI.models;

namespace StoreAPI.Database;
public sealed class StoreDB
{
    public static void CreateMysql()
    {
        Categories categories = Categories.Instance;

        IEnumerable<Category> categoryList = categories.GetCategories();

        var products = new List<Product>{

            new Product("Cinder", "Marissa Meyer","https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768889_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            9500 ,categoryList.Single(category => category.IdCategory == 3),1),
            new Product("Scarlet", "Marissa Meyer", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768896_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            9500, categoryList.Single(category => category.IdCategory == 3), 2),
            new Product("Cress", "Marissa Meyer", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768902_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            9500, categoryList.Single(category => category.IdCategory == 3), 3),
            new Product("Winter", "Marissa Meyer", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768926_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 3), 4),
            new Product("Fairest", "Marissa Meyer", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250774057_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            8700, categoryList.Single(category => category.IdCategory == 3), 5),
            new Product("La Sociedad de la Nieve", "Pablo Vierci", "https://www.libreriainternacional.com/media/catalog/product/9/7/9786070794162_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            12800, categoryList.Single(category => category.IdCategory == 6), 6),
            new Product("En Agosto nos vemos", "Gabriel García Márquez", "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073911290_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            14900, categoryList.Single(category => category.IdCategory == 7), 7),
            new Product("El estrecho sendero entre deseos", "Patrick Rothfuss", "https://www.libreriainternacional.com/media/catalog/product/9/7/9789585457935_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            12800, categoryList.Single(category => category.IdCategory == 7), 8),
            new Product("Alas de Sangre", "Rebecca Yarros", "https://www.libreriainternacional.com/media/catalog/product/9/7/9788408279990_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            19800, categoryList.Single(category => category.IdCategory == 1), 9),
            new Product("Corona de Medianoche", "Sarah J. Mass", "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073143691_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            15800, categoryList.Single(category => category.IdCategory == 4), 10),
            new Product("Carta de Amor a los Muertos", "Ava Dellaira", "https://m.media-amazon.com/images/I/41IETN4YxGL._SY445_SX342_.jpg",
            8900, categoryList.Single(category => category.IdCategory == 2), 11),
            new Product("Alicia en el país de las Maravillas", "Lewis Carrol", "https://www.libreriainternacional.com/media/catalog/product/9/7/9788415618713_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            7900, categoryList.Single(category => category.IdCategory == 7), 0),
            new Product("Alicia a través del Espejo", "Lewis Carrol", "https://www.libreriainternacional.com/media/catalog/product/9/7/9788417430429_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            7900, categoryList.Single(category => category.IdCategory == 7), 12),
            new Product("Crecent City 1 House Of Earth And Blood", "Sarah J. Maas", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781635574043_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            19800, categoryList.Single(category => category.IdCategory == 7), 13),
            new Product("Crescent City 2 House Of Sky And Breath", "Sarah J. Maas", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781635574074_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            19800, categoryList.Single(category => category.IdCategory == 7), 14),
            new Product("Crescent City 3 House Of Flame And Shadow", "Sarah J. Maas", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781635574104_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            19800, categoryList.Single(category => category.IdCategory == 7), 15),
            new Product("Harry Potter And The Sorcerers Stone", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878929_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            9900, categoryList.Single(category => category.IdCategory == 1), 16),
            new Product("Harry Potter And The Chamber Of Secrets", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878936_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            9900, categoryList.Single(category => category.IdCategory == 1), 17),
            new Product("Harry Potter And The Prisoner Of Azkaban", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878943_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            9900, categoryList.Single(category => category.IdCategory == 1), 18),
            new Product("Harry Potter And The Goblet Of Fire", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878950_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 1), 19),
            new Product("Harry Potter And The Order Of The Phoenix", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878967_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 1), 20),
            new Product("Harry Potter And The Half-Blood Prince", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878974_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 1), 21),
            new Product("Harry Potter And The Deathly Hallows", "J.K Rowling", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781338878981_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            12800, categoryList.Single(category => category.IdCategory == 1), 22),
            new Product("The Hunger Games", "Suzzane Collins", "https://www.libreriainternacional.com/media/catalog/product/9/7/9780439023528_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 9), 23),
            new Product("Catching Fire", "Suzzane Collins", "https://www.libreriainternacional.com/media/catalog/product/9/7/9780545586177_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 9), 24),
            new Product("Mockingjay", "Suzzane Collins", "https://www.libreriainternacional.com/media/catalog/product/9/7/9780545663267_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 9), 25),
            new Product("Ballad Of Songbirds And Snakes", "Suzzane Collins", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781339016573_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 9), 26),
            new Product("Kingkiller Chronicle 1 The Name Of The Wind", "Patrick Rothfuss", "https://www.libreriainternacional.com/media/catalog/product/9/7/9780756404741_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            8700, categoryList.Single(category => category.IdCategory == 8), 27),
            new Product("Kingkiller 2 The Wise Mans Fear", "Patrick Rothfuss", "https://www.libreriainternacional.com/media/catalog/product/9/7/9780756404734_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 8), 28),
            new Product("Slow Regard Of Silent Things", "Patrick Rothfuss", "https://www.libreriainternacional.com/media/catalog/product/9/7/9780756411329_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            11900, categoryList.Single(category => category.IdCategory == 8), 29),
            new Product("Girl On The Train", "Paula Hawkins", "https://www.libreriainternacional.com/media/catalog/product/9/7/9781594634024_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
            13900, categoryList.Single(category => category.IdCategory == 5), 30),
            new Product ("Bookmarks", "Perfect for not to lose where your story goes", "https://i.ibb.co/g9nX9C8/1.png",9500, categoryList.Single(category => category.IdCategory == 10), 31),
            new Product("Pins", "Adding a touch of literary flair to any outfit or accessory", "https://i.ibb.co/NYj1VrY/2.png", 9500, categoryList.Single(category => category.IdCategory == 10), 32),
            new Product ( "Necklace", "A beautifull Necklace for all day wear", "https://i.ibb.co/GxYjrmz/3.png", 9500, categoryList.Single(category => category.IdCategory == 10), 33)
        };
        using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
        {
            connection.Open();


            // Create the products table if it does not exist  //;  description VARCHAR(100)  NOT NULL,
            string createTableQuery = @"

                    DROP DATABASE IF EXISTS store; 
                    CREATE DATABASE store;
                    use store;
                    
                    CREATE TABLE IF NOT EXISTS products (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100)  NOT NULL,
                        description TEXT  NOT NULL,
                        price DECIMAL(10, 2)  NOT NULL,
                        idCategory INT  NOT NULL,
                        imgUrl VARCHAR(255)  NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS paymentMethod (
                        id INT PRIMARY KEY NOT NULL,
                        method_name VARCHAR(50) NOT NULL,
                        active BOOLEAN NOT NULL DEFAULT FALSE 
                        );
                    
                    CREATE TABLE IF NOT EXISTS sales (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        purchase_date DATETIME NOT NULL,
                        total DECIMAL(10, 2) NOT NULL,
                        payment_method  INT NOT NULL,
                        purchase_number VARCHAR(50) NOT NULL,
                        FOREIGN KEY (payment_method) REFERENCES paymentMethod(id)
                    );

                    CREATE TABLE IF NOT EXISTS saleLines (
                        sale_id INT,
                        product_id INT,
                        quantity INT NOT NULL,
                        final_price DECIMAL(10, 2)  NOT NULL,
                        PRIMARY KEY (sale_id, product_id),
                        FOREIGN KEY (sale_id) REFERENCES sales(Id),
                        FOREIGN KEY (product_id) REFERENCES products(id)
                    );   
                    INSERT INTO paymentMethod (id, method_name)
                            VALUES (0, 'Efectivo'), (1, 'Sinpe');
                    
                  CREATE TABLE messages (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        content TEXT NOT NULL,
                        timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        active BOOLEAN NOT NULL DEFAULT FALSE 
                    ) ENGINE=InnoDB;

                    INSERT INTO messages (content, timestamp)
                    VALUES
                        ('Hola', '2024-06-22 14:57:35'),
                        ('Prueba para campaing', '2024-06-22 15:02:18'),
                        ('Notification prove', '2024-06-22 15:03:38');


                    INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                    VALUES 
                        ('2024-05-01 10:00:00', 5000,  1, 'ABD12345'), ('2024-05-02 09:00:00', 15000,  1, 'NEW12345'),
                        ('2024-05-03 09:00:00', 15200,  1, 'WER8352'), ('2024-05-03 10:30:00', 8200,  0, 'QXR9235'),
                        ('2024-05-04 10:30:00', 8400,  0, 'YUI9204'), ('2024-05-05 11:45:00', 9700,  1, 'ZPL4110'),
                        ('2024-05-06 11:45:00', 9900,  1, 'CDE4682'), ('2024-05-06 12:00:00', 11200,  0, 'WSG7589'),
                        ('2024-05-07 12:00:00', 11500,  0, 'BGT5726'), ('2024-05-08 14:15:00', 13500,  1, 'AIB6427'),
                        ('2024-05-09 14:15:00', 8700,  0, 'OUY8534'), ('2024-05-09 14:15:00', 13800,  1, 'UIO3014'),
                        ('2024-05-09 15:30:00', 8700,  0, 'NEW97531'), ('2024-05-10 11:30:00', 7520,  0, 'GML54321'),
                        ('2024-05-10 15:30:00', 8900,  0, 'NEW53148'), ('2024-05-10 15:30:00', 8900,  0, 'JKL7809'),
                        ('2024-05-10 16:45:00', 7800,  1, 'NEW86420'), ('2024-05-10 16:45:00', 7800,  1, 'XVD7065'),
                        ('2024-05-11 13:45:00', 10050,  1, 'GKS98765'), ('2024-05-11 13:45:00', 10050,  1, 'GKS98765'),
                        ('2024-05-12 09:15:00', 63000,  1, 'XYZ12345'), ('2024-05-12 16:45:00', 7500,  1, 'ASD6123'),
                        ('2024-05-12 18:00:00', 9500,  0, 'NEW75309'), ('2024-05-12 18:00:00', 9500,  0, 'IKJ2491'),
                        ('2024-05-13 18:00:00', 9300,  0, 'NHJ4298'), ('2024-05-13 19:15:00', 8600,  1, 'NEW64257'),
                        ('2024-05-14 19:15:00', 8600,  1, 'RTM1473'), ('2024-05-15 19:15:00', 9700,  1, 'ZXW3749'),
                        ('2024-05-15 20:30:00', 7300,  0, 'DCE6301'), ('2024-05-16 09:00:00', 14000,  1, 'NEW98765'),
                        ('2024-05-16 20:30:00', 7200,  0, 'NEW97531'),('2024-05-16 20:30:00', 7200,  0, 'QWE9573'),
                        ('2024-05-17 10:30:00', 8100,  0, 'LKJ8902'), ('2024-05-18 09:00:00', 15400,  1, 'NEW86420'),
                        ('2024-05-18 11:45:00', 9500,  1, 'QRS98765'),('2024-05-19 10:30:00', 8600,  0, 'NEW75309'),
                        ('2024-05-19 10:30:00', 8600,  0, 'UIO1394'), ('2024-05-19 14:15:00', 8500,  1, 'NEW13579'),
                        ('2024-05-20 12:00:00', 11400,  0, 'NEW97531'),('2024-05-21 11:45:00', 10000,  1, 'NEW64257'),
                        ('2024-05-22 12:00:00', 11800,  0, 'NEW53148'),('2024-05-22 14:15:00', 13700,  1, 'NEW86420'),
                        ('2024-05-23 15:30:00', 8800,  0, 'NEW75309'), ('2024-05-24 16:45:00', 7700,  1, 'NEW64257'),
                        ('2024-05-24 16:45:00', 7800,  1, 'NOP86420'), ('2024-05-25 18:00:00', 8900,  0, 'DEF75309'),
                        ('2024-05-26 18:00:00', 9400,  0, 'NEW53148'), ('2024-05-27 19:15:00', 9600,  1, 'NEW98765'),
                        ('2024-05-27 19:15:00', 9600,  1, 'ABC64257'), ('2024-05-27 20:30:00', 7300,  0, 'MNO53148'),
                        ('2024-05-28 20:30:00', 7200,  0, 'NEW24680');
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
                                INSERT INTO products (name, description, price, idCategory, imgUrl)
                                VALUES (@name, @description, @price, @idCategory , @imgUrl);";

                        using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", product.Name);
                            insertCommand.Parameters.AddWithValue("@description", product.Description);
                            insertCommand.Parameters.AddWithValue("@price", product.Price);
                            insertCommand.Parameters.AddWithValue("@idCategory", product.ProductCategory.IdCategory);
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
    internal static async Task<List<Dictionary<string, string>>> RetrieveDatabaseInfoAsync()
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
                            string? columnValue = reader.GetValue(i).ToString();
                            row[columnName] = columnValue;
                        }
                        databaseInfo.Add(row);
                    }

                }
            }
        }

        return databaseInfo;
    }
}
