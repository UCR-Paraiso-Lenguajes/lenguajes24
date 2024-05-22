using MySqlConnector;
using ShopApi.Models;

public sealed class ProductDB
{
    public ProductDB() { }



    public static void insertProduct(Product product)
    {
        string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Begin a transaction
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // name VARCHAR(100),
                    // price DECIMAL(10, 2),
                    // imgSource VARCHAR(100)
                    string insertProductQuery = @"
                            INSERT INTO products (name, price, imgSource, category)
                            VALUES (@name, @price, @imgSource, @category);";

                    using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
                    {
                        insertCommand.Parameters.AddWithValue("@name", product.name);
                        insertCommand.Parameters.AddWithValue("@price", product.price);
                        insertCommand.Parameters.AddWithValue("@imgSource", product.imgSource);
                        insertCommand.Parameters.AddWithValue("@category", product.category);
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

    internal static IEnumerable<Product> getProducts()
    {

        List<Product> products = new List<Product>();
        List<Dictionary<string, string>> databaseInfo = new List<Dictionary<string, string>>();
        string ConnectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";


        using (var connection = new MySqlConnection(ConnectionString))
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
                            if(columnValue != null) row[columnName] = columnValue;
                        }
                        databaseInfo.Add(row);
                    }

                }
            }
        }

        foreach (var row in databaseInfo)
        {
            if (row.ContainsKey("id") && row.ContainsKey("price"))
            {
                if (int.TryParse(row["id"], out int id) &&
                    decimal.TryParse(row["price"], out decimal price) &&
                    int.TryParse(row["category"], out int categoryId))

                {
                    string name = row["name"];
                    string imgSrc = row["imgSource"];

                    Category category = CategoriesLogic.Instance.GetCategories().SingleOrDefault(c => c.id == categoryId);

                    if (!categoryId.Equals(default(Category)))
                    {
                        Product product = new Product
                        {
                            id = id,
                            name = name,
                            price = price,
                            category = categoryId,
                            imgSource = imgSrc
                        };

                        products.Add(product);
                    }
                    else
                    {
                        throw new Exception($"No se encontró la categoría correspondiente al ID {categoryId}.");
                    }
                }
            }
        }

        return products;
    }

}