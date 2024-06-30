using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace ApiLab7;

public class SaleData
{
    public static void InsertSales()
    {
        List<Product> products = Db.GetProducts();

        Sale sale;

        var dates = new[] { "20242904", "20243004", "20240604", "20240704" };

        bool restart = false;

        string insertSaleQuery =
            @"USE andromeda_store;
        INSERT INTO dbo.sales (address, purchase_amount, payment_method_id, sale_date, purchase_number, confirmed) 
        VALUES (@address, @purchaseAmount, @paymentMethod, @saleDate, @purchaseNumber, 0); SELECT SCOPE_IDENTITY()";

        string insertSaleLineQuery =
            @"USE andromeda_store;
        INSERT INTO sale_line(sale_id, product_Id, unit_price) 
        VALUES(@id, @product, @price);";

        IEnumerable<CartProduct> productsSale;

        for (int i = 0; i < 8; i++)
        {
            if (i == 4)
                restart = true;
            productsSale = Enumerable
                .Range(0, products.Count)
                .Where(index => index == i || index == (i + 1))
                .Select(index => new CartProduct
                {
                    Id = products[index].Uuid,
                    Price = products[index].Price,
                    Quantity = 1 // Assuming 1 for simplicity, you may want to change this based on your logic
                })
                .ToList();
            decimal amount = products.ElementAt(i).Price + products.ElementAt(i + 1).Price;
            PaymentMethods payment = PaymentMethods.Find((PaymentMethods.Type)0);
            sale = Sale.Build(productsSale, "direccion", amount, payment, "0010" + i, "");

            using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
            {
                connection.Open();

                try
                {
                    int generatedSaleId;
                    string format = "yyyyddMM";
                    using (SqlCommand command = new SqlCommand(insertSaleQuery, connection))
                    {
                        command.Parameters.AddWithValue("@address", sale.Address);
                        command.Parameters.AddWithValue("@purchaseAmount", sale.Amount);
                        command.Parameters.AddWithValue(
                            "@paymentMethod",
                            sale.PaymentMethod.PaymentType
                        );
                        command.Parameters.AddWithValue(
                            "@saleDate",
                            DateTime.ParseExact(
                                restart ? dates[3] : dates[i],
                                format,
                                CultureInfo.InvariantCulture
                            )
                        );
                        command.Parameters.AddWithValue("@purchaseNumber", sale.PurchaseNumber);

                        generatedSaleId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    using (SqlCommand command = new SqlCommand(insertSaleLineQuery, connection))
                    {
                        foreach (CartProduct product in sale.CartProducts)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", generatedSaleId);
                            command.Parameters.AddWithValue("@product", product.Id);
                            command.Parameters.AddWithValue("@price", product.Price);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    public async Task<IEnumerable<KeyValuePair<string, decimal>>> GetSalesByWeekAsync(
        DateTime dateWeek
    )
    {
        Dictionary<string, decimal> salesByWeek = [];
        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();
            try
            {
                string query =
                    @"USE andromeda_store;
                    WITH AllDaysOfWeek AS (
                        SELECT 1 AS Weekday, 'Sunday' AS DayName
                        UNION SELECT 2, 'Monday'
                        UNION SELECT 3, 'Tuesday'
                        UNION SELECT 4, 'Wednesday'
                        UNION SELECT 5, 'Thursday'
                        UNION SELECT 6, 'Friday'
                        UNION SELECT 7, 'Saturday'
                    )
                    SELECT
                        ADW.DayName AS DayOfWeek,
                        COALESCE(SUM(ISNULL(purchase_amount, 0)), 0) AS TotalSales
                    FROM
                        AllDaysOfWeek ADW
                    LEFT JOIN
                        sales ON ADW.Weekday = DATEPART(WEEKDAY, sale_date) 
                        AND sale_date BETWEEN DATEADD(DAY, -DATEPART(WEEKDAY, @selectedDate) + 1, @selectedDate) 
                        AND DATEADD(DAY, 7 - DATEPART(WEEKDAY, @selectedDate), @selectedDate)
                    GROUP BY
                        ADW.Weekday, ADW.DayName
                    ORDER BY
                        ADW.Weekday;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectedDate", dateWeek);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            salesByWeek.Add(reader.GetString(0), reader.GetDecimal(1));
                        }
                    }
                }
                return salesByWeek;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime dateToFind)
    {
        List<Sale> sales = new List<Sale>();
        List<Product> products = Db.GetProducts();

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();
            try
            {
                string query =
                    @"USE andromeda_store; 
                SELECT sale_date, purchase_amount, purchase_number, product_Id
                FROM sales, sale_line WHERE sales.id = sale_line.sale_id AND sale_date = @dateParam;";

                Dictionary<(string, decimal, string), List<string>> salesDict =
                    new Dictionary<(string, decimal, string), List<string>>();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dateParam", dateToFind);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string saleDate = reader[0].ToString();
                            decimal purchaseAmount = reader.GetDecimal(1);
                            string purchaseNumber = reader.GetString(2).Trim();
                            string productId = reader.GetString(3);

                            var key = (saleDate, purchaseAmount, purchaseNumber);

                            if (!salesDict.ContainsKey(key))
                            {
                                salesDict[key] = new List<string>();
                            }

                            salesDict[key].Add(productId);
                        }

                        foreach (var kvp in salesDict)
                        {
                            string saleDate = kvp.Key.Item1;
                            decimal purchaseAmount = kvp.Key.Item2;
                            string purchaseNumber = kvp.Key.Item3;
                            List<string> productIdsStored = kvp.Value;

                            IEnumerable<Product> productsForSale = products
                                .Where(p => productIdsStored.Contains(p.Uuid.ToString().ToUpper()))
                                .ToList();

                            Sale sale = Sale.BuildForReports(
                                saleDate,
                                purchaseAmount,
                                purchaseNumber,
                                productsForSale
                            );
                            sales.Add(sale);
                        }
                    }
                }
                return sales;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
