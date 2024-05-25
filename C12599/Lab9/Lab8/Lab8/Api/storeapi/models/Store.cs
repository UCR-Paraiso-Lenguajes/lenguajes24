using System.Collections.Generic;
using System.Linq;

namespace storeapi
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(List<Product> products, int taxPercentage)
        {
            Products = products;
            TaxPercentage = taxPercentage;
        }

        public static readonly Store Instance;
        static Store()
        {
            List<Product> products = LoadProductsFromDatabase();
            Instance = new Store(products, 13); 
        }

        private static List<Product> LoadProductsFromDatabase()
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
                   
                }
               
            }

            return products;
        }
    }
}
