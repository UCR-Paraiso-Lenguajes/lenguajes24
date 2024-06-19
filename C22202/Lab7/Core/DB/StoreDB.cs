using MySqlConnector;
using ShopApi.Models;

namespace ShopApi.db;
public sealed class StoreDB
{
    public StoreDB(){}

    public static void CreateMysql()
    {
        Random random = new Random();

        var products = new List<Product>
        {
            new Product { id = 1, name = "Logitech MX Master", imgSource = "https://m.media-amazon.com/images/I/61ni3t1ryQL.__AC_SX300_SY300_QL70_FMwebp_.jpg", price = 99.99m, category = 1 },
            new Product { id = 2, name = "Razer DeathAdder", imgSource = "https://m.media-amazon.com/images/I/8189uwDnMkL._AC_UY218_.jpg", price = 49.99m, category = 1 },
            new Product { id = 3, name = "Dell UltraSharp 27", imgSource = "https://m.media-amazon.com/images/I/A1Iqr2v1SIL._AC_UY218_.jpg", price = 399.99m, category = 2 },
            new Product { id = 4, name = "Samsung Odyssey G7", imgSource = "https://m.media-amazon.com/images/I/61mGW-LbByL._AC_UY218_.jpg", price = 699.99m, category = 2 },
            new Product { id = 5, name = "Samsung 970 EVO Plus", imgSource = "https://m.media-amazon.com/images/I/71OYNmVRFhL._AC_UY218_.jpg", price = 129.99m, category = 3 },
            new Product { id = 6, name = "WD Black SN850", imgSource = "https://m.media-amazon.com/images/I/51QtwjGhQYL._AC_UY218_.jpg", price = 149.99m, category = 3 },
            new Product { id = 7, name = "Seagate Barracuda", imgSource = "https://m.media-amazon.com/images/I/71dpms8gexL._AC_UY218_.jpg", price = 59.99m, category = 4 },
            new Product { id = 8, name = "WD Blue", imgSource = "https://m.media-amazon.com/images/I/7172nw99CvL._AC_UY218_.jpg", price = 49.99m, category = 4 },
            new Product { id = 9, name = "Intel Core i9-11900K", imgSource = "https://m.media-amazon.com/images/I/71diouNMRHL._AC_UY218_.jpg", price = 539.99m, category = 5 },
            new Product { id = 10, name = "AMD Ryzen 9 5900X", imgSource = "https://m.media-amazon.com/images/I/51S-lEYQZJL._AC_UY218_.jpg", price = 499.99m, category = 5 },
            new Product { id = 11, name = "NVIDIA GeForce RTX 3080", imgSource = "https://m.media-amazon.com/images/I/61juKdIql1L._AC_UY218_.jpg", price = 799.99m, category = 6 },
            new Product { id = 12, name = "AMD Radeon RX 6800 XT", imgSource = "https://m.media-amazon.com/images/I/81-70VBUexL._AC_UY218_.jpg", price = 649.99m, category = 6 },
            new Product { id = 13, name = "Corsair K95 RGB", imgSource = "https://m.media-amazon.com/images/I/71PzW7vZNUL._AC_UY218_.jpg", price = 199.99m, category = 7 },
            new Product { id = 14, name = "Logitech G Pro X", imgSource = "https://m.media-amazon.com/images/I/61ep6omO+0L._AC_UY218_.jpg", price = 129.99m, category = 7 },
            new Product { id = 15, name = "Corsair Vengeance LPX 16GB", imgSource = "https://m.media-amazon.com/images/I/41a2jzudKtL._AC_UY218_.jpg", price = 89.99m, category = 8 },
            new Product { id = 16, name = "G.Skill Trident Z RGB 32GB", imgSource = "https://m.media-amazon.com/images/I/61l4EStxhnL._AC_UY218_.jpg", price = 169.99m, category = 8 },
            new Product { id = 17, name = "HyperX Cloud II", imgSource = "https://m.media-amazon.com/images/I/71u77S3CdSL._AC_UY218_.jpg", price = 99.99m, category = 9 },
            new Product { id = 18, name = "SteelSeries Arctis 7", imgSource = "https://m.media-amazon.com/images/I/719xhYDZj9L._AC_UY218_.jpg", price = 149.99m, category = 9 },
            new Product { id = 19, name = "Corsair RM850x", imgSource = "https://m.media-amazon.com/images/I/71CnFDhomCL._AC_UY218_.jpg", price = 129.99m, category = 10 },
            new Product { id = 20, name = "EVGA SuperNOVA 750 G5", imgSource = "https://m.media-amazon.com/images/I/71POUO-MQCL._AC_UY218_.jpg", price = 139.99m, category = 10 },
            new Product { id = 21, name = "NZXT H510", imgSource = "https://m.media-amazon.com/images/I/71SIs5kxpYL._AC_UY218_.jpg", price = 69.99m, category = 11 },
            new Product { id = 22, name = "Corsair iCUE 4000X RGB", imgSource = "https://m.media-amazon.com/images/I/81BJl9d8iHL._AC_UY218_.jpg", price = 119.99m, category = 11 },
            new Product { id = 23, name = "ASUS ROG Strix Z790-A", imgSource = "https://m.media-amazon.com/images/I/819AN0kVmJL._AC_UY218_.jpg", price = 329.99m, category = 12 },
            new Product { id = 24, name = "MSI MPG B550 Gaming Edge", imgSource = "https://m.media-amazon.com/images/I/91oChAepmRL._AC_UY218_.jpg", price = 189.99m, category = 12 },
            new Product { id = 25, name = "Blue Yeti USB Mic", imgSource = "https://m.media-amazon.com/images/I/61egnO8q6ZL._AC_UL320_.jpg", price = 129.99m, category = 13 },
            new Product { id = 26, name = "Rode NT-USB", imgSource = "https://m.media-amazon.com/images/I/51Qshd1JNaL._AC_UL320_.jpg", price = 169.99m, category = 13 },
            new Product { id = 27, name = "Glorious Model O", imgSource = "https://m.media-amazon.com/images/I/71iS3zaTAAL._AC_UY218_.jpg", price = 59.99m, category = 1 },
            new Product { id = 28, name = "Acer Predator X34", imgSource = "https://m.media-amazon.com/images/I/81AimUm5QYL._AC_UY218_.jpg", price = 999.99m, category = 2 },
            new Product { id = 29, name = "Kingston A2000", imgSource = "https://m.media-amazon.com/images/I/71JM21OBzhL._AC_UY218_.jpg", price = 109.99m, category = 3 },
            new Product { id = 30, name = "Toshiba X300", imgSource = "https://m.media-amazon.com/images/I/71uVWSgYBXL._AC_UY218_.jpg", price = 89.99m, category = 4 }
        };

        string connectionString = Storage.Instance.ConnectionString;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Create the products table if it does not exist
            string createTableQuery = @"
                DROP DATABASE IF EXISTS store;
                CREATE DATABASE store;
                use store;
                
                CREATE TABLE IF NOT EXISTS products (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(100),
                    price DECIMAL(10, 2),
                    imgSource VARCHAR(255),
                    category INT,
                    deleted INT DEFAULT 0
                );
                
                CREATE TABLE IF NOT EXISTS sales (
                    sale_id INT AUTO_INCREMENT PRIMARY KEY,
                    purchase_date DATETIME NOT NULL,
                    total DECIMAL(10, 2) NOT NULL,
                    payment_method ENUM('0', '1'),
                    purchase_number VARCHAR(50) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS sale_product (
                    sale_id INT,
                    product_id INT,
                    price DECIMAL(10, 2),
                    PRIMARY KEY (sale_id, product_id),
                    FOREIGN KEY (sale_id) REFERENCES sales(sale_id),
                    FOREIGN KEY (product_id) REFERENCES products(id)
                );";


            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                int result = command.ExecuteNonQuery();
                bool dbNoCreated = result < 0;
                if(dbNoCreated)
                    throw new Exception("Error creating the bd");
            }
        }
        foreach (Product product in products)
        {
            ProductDB.insertProduct(product);
        }
    }
}