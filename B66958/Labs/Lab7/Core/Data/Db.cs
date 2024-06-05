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
        List<Product> products = Products.Instance.GetProducts().ToList();
        string insertQuery =
            @"USE andromeda_store;
            INSERT INTO dbo.products (id, name, description, image_Url, price, category) 
            VALUES (@id, @name, @description, @imageUrl, @price, @category)";

        for (int i = 0; i < products.Count(); i++)
        {
            using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", products.ElementAt(i).Uuid);
                    command.Parameters.AddWithValue("@name", products.ElementAt(i).Name);
                    command.Parameters.AddWithValue("@description", products.ElementAt(i).Description);
                    command.Parameters.AddWithValue("@imageUrl", products.ElementAt(i).ImageUrl);
                    command.Parameters.AddWithValue(
                        "@price",
                        products.ElementAt(i).Price * i
                    );
                    command.Parameters.AddWithValue("@category", products.ElementAt(i).Category.Id);

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
                                Category = Categories.Instance.GetCategoryById(reader.GetInt32(5))
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
