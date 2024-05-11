using System;
using System.Collections.Generic;
using storeapi.Database;

namespace storeapi.Models
{
    public sealed class Products
    {
        public IEnumerable<Product> LoadProductsFromDatabase(int categoryId)
        {
            List<string[]> productData = StoreDB.RetrieveDatabaseInfo();
            List<Product> products = new List<Product>();

            Categories categories = new Categories(); // Instancia de la lista de categorías disponibles

            foreach (string[] row in productData)
            {
                if (ValidateProductRow(row))
                {
                    int id = int.Parse(row[0]);
                    string name = row[1];
                    decimal price = decimal.Parse(row[2]);
                    string imageUrl = row[3];
                    string description = row[4];
                    int rowCategoryId = int.Parse(row[5]);

                    // Verificar si el ID de la categoría del producto coincide con el ID de la categoría deseada
                    bool categoriaBuscada = rowCategoryId == categoryId;
                    
                    if (categoriaBuscada)
                    {
                        // Buscar la categoría correspondiente en la lista de categorías disponibles
                        Category category = categories.GetCategoryById(rowCategoryId);

                    
                        {
                            // Crear un nuevo producto y asignar la categoría encontrada
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
                throw new ArgumentException("Invalid product ID.");
            }

            if (string.IsNullOrWhiteSpace(row[1]))
            {
                throw new ArgumentException("Product name is null or empty.");
            }

            if (!int.TryParse(row[5], out _))
            {
                throw new ArgumentException("Invalid category ID.");
            }

            return true;
        }
    }
}