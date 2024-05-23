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

        public static readonly Store Instance = CreateInstance();

        private static Store CreateInstance()
        {
            IEnumerable<Product> products = LoadProductsFromDatabase();
            int taxPercentage = 13; 

            return new Store(products, taxPercentage);
        }

        private static IEnumerable<Product> LoadProductsFromDatabase()
        {
            List<string[]> productData = StoreDB.RetrieveDatabaseInfo();
            List<Product> products = new List<Product>();
            Categories categories = new Categories(); 

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

                  
                    Category category = categories.GetCategoryById(categoryId);

                   
                    {
                       
                        Product product = new Product
                        {
                            id = id,
                            Name = name,
                            Price = price,
                            Description = description,
                            ImageUrl = imageUrl,
                            Category = category
                        };

                        products.Add(product);
                   
                        
                    }
                }
            }

            return products;
        }

        private static bool ValidateProductRow(string[] row)
        {
            // Validar que el arreglo de datos de producto tenga al menos 6 elementos
            if (row == null || row.Length < 6)
            {
                return false;
            }

            // Validar que el ID del producto sea un entero válido
            if (!int.TryParse(row[0], out _))
            {
                return false;
            }

            // Validar que el precio del producto sea un decimal válido
            if (!decimal.TryParse(row[2], out _))
            {
                return false;
            }

            // Validar que el nombre del producto no esté vacío o nulo
            if (string.IsNullOrWhiteSpace(row[1]))
            {
                return false;
            }

            // Validar que el ID de categoría sea un entero válido
            if (!int.TryParse(row[5], out _))
            {
                return false;
            }

            return true;
        }
    }
}
