using System;
using System.Collections.Generic;
using storeapi.Database;

namespace storeapi.Models
{
    public sealed class Store
    {
        public IEnumerable<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(IEnumerable<Product> products, int taxPercentage)
        {
            Products = products;
            TaxPercentage = taxPercentage;
        }

        public static readonly Store Instance;
        static Store()
        {
            IEnumerable<Product> products = LoadProductsFromDatabase();
            Instance = new Store(products, 13); 
        }

        private static IEnumerable<Product> LoadProductsFromDatabase()
        {
            List<string[]> productData = StoreDB.RetrieveDatabaseInfo();
            List<Product> products = new List<Product>();

            foreach (string[] row in productData)
            {
                if (row.Length >= 5) 
                {
                    if (int.TryParse(row[0], out int id) &&
                        decimal.TryParse(row[2], out decimal price))
                    {
                        string name = row[1];
                        string description = row[4];
                        string imageUrl = row[3];

                        // Validate product data before creating a new instance
                        if (id <= 0)
                        {
                            throw new ArgumentException("Invalid product ID: must be a positive integer.");
                        }

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            throw new ArgumentException("Product name cannot be null or empty.");
                        }

                        if (price <= 0)
                        {
                            throw new ArgumentException("Product price must be a positive decimal value.");
                        }

                        Product product = new Product
                        {
                            id = id,
                            Name = name,
                            Price = price,
                            Description = description,
                            ImageUrl = imageUrl
                        };

                        products.Add(product);
                    }
                    else
                    {
                        throw new ArgumentException($"Error parsing product data: {string.Join(", ", row)}");
                    }
                }
            }

            return products;
        }
    }
}
