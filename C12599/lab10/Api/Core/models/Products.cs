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

        private bool ValidateProductRow(string[] row)
        {
            if (row == null || row.Length < 6)
            {
                throw new ArgumentException("Invalid product data row: Insufficient columns.");
            }

            if (!int.TryParse(row[0], out _))
            {
                throw new ArgumentException("Invalid product ID: not a valid integer.");
            }

            if (!decimal.TryParse(row[2], out _))
            {
                throw new ArgumentException("Invalid product price: not a valid decimal value.");
            }

            if (string.IsNullOrWhiteSpace(row[1]))
            {
                throw new ArgumentException("Product name is null or empty.");
            }

            if (!int.TryParse(row[5], out _))
            {
                throw new ArgumentException("Invalid category ID: not a valid integer.");
            }

            return true;
        }
    }
}
