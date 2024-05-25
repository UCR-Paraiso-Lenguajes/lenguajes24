using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApiLab7;

public class Db
{
    public static string DbConnectionString = "Data Source=163.178.173.130;" +
            "User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;";

    public static void CreateDB()
    {
        string[] creationQueries = {
            "IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'andromeda_store') DROP DATABASE andromeda_store;",
            "CREATE DATABASE andromeda_store;",
            "USE andromeda_store;",
            "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'products') " +
            "BEGIN " +
            "CREATE TABLE products (id INT PRIMARY KEY IDENTITY, product_id CHAR(36), name VARCHAR(100), description VARCHAR(100), "+
            "image_Url VARCHAR(MAX), price DECIMAL(10, 2)); END;",
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

    public bool productsExist(){
        string query = "USE andromeda_store; select count(*) from products";
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
        var productsData = new[]
        {
            new { name = "Producto", description = "Gaming Mouse", imageUrl = "https://cdn.mos.cms.futurecdn.net/rfphfWvEc3PL2wfPJvZGiP.jpg", price = 75.0 },
            new { name = "Producto", description = "Monitor", imageUrl = "https://images.samsung.com/is/image/samsung/assets/nz/members/article-assets/gaming-monitors/img-kv-2.jpg?$ORIGIN_JPG$", price = 700.0 },
            new { name = "Producto", description = "Mousepad", imageUrl = "https://media.steelseriescdn.com/blog/posts/how-to-choose-your-mousepad/38569118cb1443abb9b88cf9b3f10da0.jpg", price = 30.0 },
            new { name = "Producto", description = "Gaming keyboard", imageUrl = "https://png.pngtree.com/png-vector/20220728/ourmid/pngtree-gaming-keyboard-rgb-effect-png-image_6087818.png", price = 30.0 }
        };

        string insertQuery = @"USE andromeda_store;
            INSERT INTO dbo.products (product_id, name, description, image_Url, price) 
            VALUES (@id, @name, @description, @imageUrl, @price)";

        for (int i = 1; i <= 40; i++)
        {
            var productData = productsData[(i - 1) % productsData.Length];

            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", $"Producto {i}");
                    command.Parameters.AddWithValue("@description", productData.description);
                    command.Parameters.AddWithValue("@imageUrl", productData.imageUrl);
                    command.Parameters.AddWithValue("@price", Convert.ToDecimal(productData.price) * i);

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
            
                string query = @"USE andromeda_store; SELECT product_id, name, description, image_Url, price FROM products;";
            
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Uuid = new Guid(reader[0].ToString()),
                                Name = reader[1].ToString(),
                                Description = reader[2].ToString(),
                                ImageUrl = reader[3].ToString(),
                                Price = (decimal)reader[4]
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