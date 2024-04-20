using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;


namespace geekstore_api.DataBase
{
public sealed class StoreDb
{
        
public static void CrearDatos()
{
    string connectionString = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                string createTableQuery = @"
                    DROP TABLE IF EXISTS salesLine;
                    DROP TABLE IF EXISTS sales;
                    DROP TABLE IF EXISTS products;
                    CREATE TABLE IF NOT EXISTS products (
                         id INT AUTO_INCREMENT PRIMARY KEY,
                         name VARCHAR(100),
                         description VARCHAR(255),
                         price DECIMAL(10, 2),
                         imageURL VARCHAR(255)
                     );
                     CREATE TABLE IF NOT EXISTS sales (
                         purchase_number VARCHAR(30) NOT NULL PRIMARY KEY,
                         purchase_date DATETIME NOT NULL,
                         total DECIMAL(10, 2) NOT NULL,
                         payment_type INT
                     );

                     CREATE TABLE IF NOT EXISTS salesLine(
                         id INT AUTO_INCREMENT PRIMARY KEY,
                         purchase_id VARCHAR(30) NOT NULL,
                         product_id INT,
                         price DECIMAL(10, 2),
                         FOREIGN KEY (purchase_id) REFERENCES sales(purchase_number),
                         FOREIGN KEY (product_id) REFERENCES products(id)
                     );
                     CREATE TABLE IF NOT EXISTS paymentMethod (
                         id INT PRIMARY KEY,
                         payment_type VARCHAR(50)
                     );";

                using (var command = new MySqlCommand(createTableQuery, connection, transaction))
                {
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        agregarProductos();
    }
}

    internal static void agregarProductos(){

            string connectionString = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";

            var products = new List<Product>
            {
                new Product { id = 1,  name = "Gamer tools", description ="Perifericos disponibles de diferentes diseños", price= 30, imageUrl = "https://png.pngtree.com/png-vector/20220725/ourmid/pngtree-gaming-equipment-computer-peripheral-device-png-image_6064567.png" },
                new Product { id = 2, name = "Portatil", description ="Portatiles para todo tipo de usuario y necesidad", price= 625, imageUrl = "https://sitechcr.com/wp-content/uploads/2016/06/A15_i781T3GSW10s4.jpg" },
                new Product { id = 3,  name= "Figuras MHA", description= "Decora tu lugar preferido a tu propio estilo", price=44, imageUrl= "https://m.media-amazon.com/images/I/61lHgRfaG2L._AC_UF894,1000_QL80_.jpg" },
                new Product { id = 4, name = "Hoodie Viñeta", description = "Busca tu diseño personalizado y característico", price = 55, imageUrl = "https://eg.jumia.is/unsafe/fit-in/500x500/filters:fill(white)/product/28/651172/1.jpg?7281" },
                new Product { id = 5, name = "Shonen Jump", description = "Mantente al día con las publicaciones", price = 40, imageUrl = "https://pbs.twimg.com/media/FslBjwGWIAElbQv.jpg:large" },
                new Product { id = 6, name = "FFVII", description = "Compra los últimos lanzamientos", price = 49, imageUrl = "https://sm.ign.com/ign_ap/cover/f/final-fant/final-fantasy-vii-remake-part-2_gq8f.jpg" },
                new Product { id = 7, name = "Kimetsu DVD", description = "Get the best releases", price = 28, imageUrl = "https://pisces.bbystatic.com/image2/BestBuy_US/images/products/9111c4a7-8d9d-47c6-adbe-424a9b2dc5f4.jpg" },
                new Product { id = 8, name = "Vinyl Record", description = "Find a wide variety of genres", price = 23, imageUrl = "https://static.dezeen.com/uploads/2022/09/bioplastic-record-pressing_dezeen_2364_col_1.jpg" },
                new Product { id = 9, name = "SSD Drives", description = "Upgrade your PC with the latest technology", price = 38, imageUrl = "https://c1.neweggimages.com/productimage/nb640/20-250-088-V03.jpg" },
                new Product { id = 10, name = "Merch", description = "Merchandise from your favorite event", price= 28, imageUrl = "https://members.asicentral.com/media/32573/tshirtathome-616.jpg" },
                new Product { id = 11, name = "Software", description = "Your favorite tools", price = 40, imageUrl = "https://m.media-amazon.com/images/I/81CucSxYsJL._AC_UF1000,1000_QL80_.jpg" },
                new Product { id = 12, name = "Bento Box", description = "Get your favorite bentos", price = 13, imageUrl = "https://m.media-amazon.com/images/I/51O0hWbj2gL._AC_UF894,1000_QL80_.jpg" }
            };

            using (var connection = new MySqlConnection(connectionString)){

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var product in products)
                        {
                            string insertQuery = @"
                                INSERT INTO products (name, description, price, imageURL)
                                VALUES (@name, @description, @price, @imageURL)";

                            using (var command = new MySqlCommand(insertQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@name", product.name);
                                command.Parameters.AddWithValue("@description", product.description);
                                command.Parameters.AddWithValue("@price", product.price);
                                command.Parameters.AddWithValue("@imageUrL", product.imageUrl);
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
                
                connection.Close();
            }
    }


    public static List<Product> ExtraerProductosDB()
    {
    List<Product> productList = new List<Product>();
    string connectionString = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";

    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();

        string selection = "SELECT id, name, description, price, imageUrl FROM products";

        using (var command = new MySqlCommand(selection, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    productList.Add(new Product
                    {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    name = reader.GetString(reader.GetOrdinal("name")),
                    description = reader.GetString(reader.GetOrdinal("description")),
                    price = reader.GetDecimal(reader.GetOrdinal("price")),
                    imageUrl = reader.GetString(reader.GetOrdinal("imageUrl")),
                    });

                }
            }
        }
        connection.Close();
        }
        return productList;
        }

    }
 
}

