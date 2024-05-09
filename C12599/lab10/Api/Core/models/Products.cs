using System;
using System.Collections.Generic;
using storeapi.Database;

namespace storeapi.Models
{
    public sealed class Products
    {
   
       public IEnumerable<Product> LoadProductsFromDatabase(string CategoryID)
    {
        List<string[]> productData = StoreDB.RetrieveDatabaseInfo();
        List<Product> products = new List<Product>();

        foreach (string[] row in productData)
        {
            if (ValidateProductRow(row))
            {
                int id = int.Parse(row[0]);
                string name = row[1];
                decimal price = decimal.Parse(row[2]);
                string imageUrl = row[3];
                string description = row[4];
                int categoryId = int.Parse(row[5]);

                if (categoryId.ToString() == CategoryID)
                {
                    Product product = new Product
                    {
                        id = id,
                        Name = name,
                        Price = price,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoryID = categoryId
                    };

                    products.Add(product);
                }
            }
        }

        return products;
    }

        public bool ValidateProductRow(string[] row)
        {
            if (row == null || row.Length < 6)
            {
                Console.WriteLine("Invalid product data row: Insufficient columns.");
                return false;
            }

            if (!int.TryParse(row[0], out _))
            {
                Console.WriteLine($"Invalid product ID: '{row[0]}' is not a valid integer.");
                return false;
            }

            if (!decimal.TryParse(row[2], out _))
            {
                Console.WriteLine($"Invalid product price: '{row[2]}' is not a valid decimal value.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(row[1]))
            {
                Console.WriteLine("Product name is null or empty.");
                return false;
            }

            if (!int.TryParse(row[5], out _))
            {
                Console.WriteLine($"Invalid category ID: '{row[5]}' is not a valid integer.");
                return false;
            }

            return true;
        }
    }
}
