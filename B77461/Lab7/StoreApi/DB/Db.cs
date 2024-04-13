using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using StoreApi.Models;

namespace StoreApi.DB;

public class Db
{
    public static string DbConnectionString = "Data Source=163.178.173.130;" +
            "User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;";

    public static void CreateDB()
    {
        string[] creationQueries = {
            "IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'Lenguajes_Store') DROP DATABASE Lenguajes_Store;",
            "CREATE DATABASE Lenguajes_Store;",
            "USE Lenguajes_Store;",
            "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'products') " +
            "BEGIN " +
            "CREATE TABLE products (id INT PRIMARY KEY IDENTITY, product_id CHAR(36), name VARCHAR(100), description VARCHAR(100), "+
            "image_Url VARCHAR(MAX), price DECIMAL(10, 2), quantity INT); END;",
            "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'sales') " +
            "BEGIN " +
            "CREATE TABLE sales (id INT PRIMARY KEY IDENTITY, address VARCHAR(100), "
            + "purchase_amount DECIMAL(10, 2), payment_method INT, sale_date DATETIME, purchase_number VARCHAR(20)); " +
            "END;"
        };

        using (SqlConnection connection = new SqlConnection(DbConnectionString))
        {
            connection.Open();
            foreach (string query in creationQueries)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try{
                        command.ExecuteNonQuery();
                    }catch(Exception ex){
                        throw ex;
                    }
                }
            }
        }
    }

    public static bool productsExist(){
        string query = "USE Lenguajes_Store; select count(*) from products";
        using (SqlConnection connection = new SqlConnection(DbConnectionString))
        {
            // Create a new SqlCommand with the query and SqlConnection
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                int rowCount = (int)command.ExecuteScalar();

                bool tableHasProducts = rowCount > 0;

                return tableHasProducts;
            }
        }
    }

    public static void FillProducts()
    {

        if(productsExist()) return ;

        var productsData = new[]
        {
        new Product { name = "Audifonos", description = "Audifonos RGB", imageUrl = "https://tienda.starware.com.ar/wp-content/uploads/2021/05/auriculares-gamer-headset-eksa-e1000-v-surround-71-rgb-pc-ps4-verde-2331-3792.jpg", price = 60.0m, quantity = 5 },
        new Product { name = "Teclado", description = "Teclado mecánico RGB", imageUrl = "https://kuwait.gccgamers.com/razer-deathstalker-v2/assets/product.webp", price = 75.0m, quantity = 4 },
        new Product { name = "Mouse", description = "Mouse inalámbrico", imageUrl = "https://static3.tcdn.com.br/img/img_prod/374123/mouse_gamer_impact_rgb_12400_dpi_m908_redragon_29921_3_20190927170055.jpg", price = 35.0m, quantity = 3 },
        new Product { name = "Monitor", description = "Monitor LCD", imageUrl = "https://i5.walmartimages.ca/images/Large/956/188/6000199956188.jpg", price = 200.0m, quantity = 12 },
        new Product { name = "CASE", description = "Case CPU", imageUrl = "https://th.bing.com/th/id/OIP.mhKR13PBP5mQP85l2c4DWgHaHa?rs=1&pid=ImgDetMain", price = 450.0m, quantity = 11 },
        new Product { name = "MousePad", description = "MousePad HYPER X", imageUrl = "https://s3.amazonaws.com/static.spdigital.cl/img/products/new_web/1500590806008-36964857_0168832511.jpg", price = 15.0m, quantity = 10 },
        new Product { name = "Laptop", description = "Laptop ASUS", imageUrl = "https://resources.claroshop.com/medios-plazavip/s2/10252/1145258/5d13a10bac9b0-laptop-gamer-asus-rog-strix-scar-ii-i7-16gb-512gb-rtx-2070-1600x1600.jpg", price = 1000.0m, quantity = 9 },
        new Product { name = "Tarjeta de Video", description = "Tarjeta Nvidia 4060", imageUrl = "https://ddtech.mx/assets/uploads/861311bd60bf6ede94bfe7ab01e705a3.png", price = 600.0m, quantity = 8 },
        new Product { name = "Control", description = "Control STEAM", imageUrl = "https://th.bing.com/th/id/OIP.lNj-nw7kO0Q73XjkAvaQkwHaJJ?rs=1&pid=ImgDetMain", price = 150.0m, quantity = 7 },
        new Product { name = "Gafas VR", description = "Gafas VR PS4", imageUrl = "https://img.pccomponentes.com/articles/15/157238/a2.jpg", price = 500.0m, quantity = 6 },
        new Product { name = "Pantalla", description = "Pantalla LG OLED", imageUrl = "https://th.bing.com/th/id/OIP.nC89zBQSGxR8hyVnocBvlQHaGb?rs=1&pid=ImgDetMain", price = 750.0m, quantity = 4 },
        new Product { name = "Celular", description = "ASUS ROG", imageUrl = "https://www.latercera.com/resizer/E392-vfE0PVd1xTj8wEKR6Ud7Z0=/800x0/smart/cloudfront-us-east-1.images.arcpublishing.com/copesa/3QACWYB2FNENTINU4KTAXU2D2A.jpg", price = 900.0m, quantity = 13 }

        };

        string insertQuery = @"USE Lenguajes_Store;
            INSERT INTO dbo.products (product_id, name, description, image_Url, price, quantity) 
            VALUES (@id, @name, @description, @imageUrl, @price, @quantity)";

        for (int i = 1; i <= 30; i++)
        {
            var productData = productsData[(i - 1) % productsData.Length];

            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", productData.name);
                    command.Parameters.AddWithValue("@description", productData.description);
                    command.Parameters.AddWithValue("@imageUrl", productData.imageUrl);
                    command.Parameters.AddWithValue("@price", Convert.ToDecimal(productData.price) * i);
                    command.Parameters.AddWithValue("@quantity", productData.quantity);

                    command.ExecuteNonQuery();
                }
                
            }
        }
    }

    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (SqlConnection connection = new SqlConnection(DbConnectionString))
        {
            try
            {
                connection.Open();
            
                string query = @"USE Lenguajes_Store; SELECT product_id, name, description, image_Url, price, quantity FROM products;";
            
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Uuid = new Guid(reader[0].ToString()),
                                name = reader[1].ToString(),
                                description = reader[2].ToString(),
                                imageUrl = reader[3].ToString(),
                                price = (decimal)reader[4],
                                quantity = (int) reader[5]
                            };

                        products.Add(product);
                        }
                    }
                }
            
                return products;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
    }
}