using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace ApiLab7;

public class Db
{
    public string DbConnectionString { get; private set; }

    public static Db Instance;

    public static void BuildDb(string sqlServerConnectionString)
    {
        if (string.IsNullOrEmpty(sqlServerConnectionString))
            throw new ArgumentNullException($"{nameof(sqlServerConnectionString)} is required.");

        Db.Instance = new Db(sqlServerConnectionString);
    }

    private Db(string sqlServerConnectionString)
    {
        DbConnectionString = sqlServerConnectionString;
    }

    public static void CreateDB()
    {
        string[] creationQueries =
        {
            "IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'andromeda_store') DROP DATABASE andromeda_store;",
            "CREATE DATABASE andromeda_store;",
            "USE andromeda_store;",
            "CREATE TABLE products (id CHAR(36) PRIMARY KEY, name VARCHAR(100), description VARCHAR(100), "
                + "image_Url VARCHAR(MAX), price DECIMAL(10, 2), category INT);",
            "CREATE TABLE payment_method (id INT PRIMARY KEY, name VARCHAR(20), description VARCHAR(100), enabled BIT)",
            "CREATE TABLE sales (id INT PRIMARY KEY IDENTITY(1,1), address VARCHAR(100), "
                + "purchase_amount DECIMAL(10, 2), payment_method_id INT, purchase_number CHAR(6), sale_date DATETIME, "
                + "confirmed BIT, CONSTRAINT fkPaymentMethodSale FOREIGN KEY (payment_method_id) REFERENCES payment_method(id));",
            "CREATE TABLE sale_line (id INT IDENTITY(1,1) PRIMARY KEY, sale_id INT, product_Id CHAR(36), unit_price DECIMAL(10, 2), "
                + "CONSTRAINT fkSale FOREIGN KEY (sale_id) REFERENCES sales(id));",
            "CREATE TABLE sinpe_confirmation_number (id INT PRIMARY KEY, confirmation_number VARCHAR(20), "
                + "CONSTRAINT fkSinpeSaleid FOREIGN KEY (id) REFERENCES sales(id))",
        };

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.Open();
            foreach (string query in creationQueries)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }

    /* public bool productsExist()
    {
        string query = "USE andromeda_store; SELECT COUNT(*) FROM products";
        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                int rowCount = (int)command.ExecuteScalar();

                bool tableHasProducts = rowCount > 0;

                return tableHasProducts;
            }
        }
    } */

    public static void FillPaymentMethods()
    {
        string insertQuery =
            @"USE andromeda_store;
            INSERT INTO dbo.payment_method (id, name, description, enabled) 
            VALUES (0, 'CASH', NULL, 1), (1, 'SINPE', NULL, 1)";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static void FillProducts()
    {
        List<int> categories = Categories
            .Instance.GetCategories()
            .Select(category => category.Id)
            .ToList();

        Random rand = new Random();

        var productsData = new[]
        {
            new
            {
                name = "Producto",
                description = "Gaming Mouse",
                imageUrl = "https://cdn.mos.cms.futurecdn.net/rfphfWvEc3PL2wfPJvZGiP.jpg",
                price = 75.0
            },
            new
            {
                name = "Producto",
                description = "Monitor",
                imageUrl = "https://images.samsung.com/is/image/samsung/assets/nz/members/article-assets/gaming-monitors/img-kv-2.jpg?$ORIGIN_JPG$",
                price = 700.0
            },
            new
            {
                name = "Producto",
                description = "Mousepad",
                imageUrl = "https://media.steelseriescdn.com/blog/posts/how-to-choose-your-mousepad/38569118cb1443abb9b88cf9b3f10da0.jpg",
                price = 30.0
            },
            new
            {
                name = "Producto",
                description = "Gaming keyboard",
                imageUrl = "https://png.pngtree.com/png-vector/20220728/ourmid/pngtree-gaming-keyboard-rgb-effect-png-image_6087818.png",
                price = 30.0
            }
        };

        string insertQuery =
            @"USE andromeda_store;
            INSERT INTO dbo.products (id, name, description, image_Url, price, category) 
            VALUES (@id, @name, @description, @imageUrl, @price, @category)";

        for (int i = 1; i <= 40; i++)
        {
            var productData = productsData[(i - 1) % productsData.Length];

            int randomIndex = rand.Next(categories.Count);

            using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", $"Producto {i}");
                    command.Parameters.AddWithValue("@description", productData.description);
                    command.Parameters.AddWithValue("@imageUrl", productData.imageUrl);
                    command.Parameters.AddWithValue(
                        "@price",
                        Convert.ToDecimal(productData.price) * i
                    );
                    command.Parameters.AddWithValue("@category", categories.ElementAt(randomIndex));

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public List<PaymentMethods> GetPaymentMethods()
    {
        List<PaymentMethods> paymentMethods = new List<PaymentMethods>();
        using (SqlConnection connection = new SqlConnection(DbConnectionString))
        {
            try
            {
                connection.Open();

                string query = @"USE andromeda_store; SELECT id FROM payment_method;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int paymentMethodId = Convert.ToInt32(reader[0].ToString());
                            PaymentMethods payment = PaymentMethods.Find(
                                (PaymentMethods.Type)paymentMethodId
                            );
                            paymentMethods.Add(payment);
                        }
                    }
                }

                return paymentMethods;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            try
            {
                connection.Open();

                string query = @"USE andromeda_store; SELECT * FROM products;";

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
                                Price = (decimal)reader[4],
                                CategoryId = reader.GetInt32(5)
                            };

                            products.Add(product);
                        }
                    }
                }

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
