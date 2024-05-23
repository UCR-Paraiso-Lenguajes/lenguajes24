using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using StoreAPI.Database;

namespace StoreAPI.models;

public sealed class Store
{
    public List<Product> Products { get; private set; } //hacer un diccionario de productos por categoria
    public IEnumerable<Category> CategoriesList { get; private set; }
    public int TaxPercentage { get; private set; }

    private Store(List<Product> products, IEnumerable<Category> categories, int TaxPercentage)
    {
        if (products == null || products.Count == 0) throw new ArgumentException($"The list of {nameof(products)} cannot be null or empty.");
        if (categories == null || !categories.Any()) throw new ArgumentException($"The collection of {nameof(categories)} cannot be null or empty.");
        if (TaxPercentage < 0) throw new ArgumentException($"The {nameof(TaxPercentage)} must be greater than or equal to zero.");

        Products = products;
        CategoriesList = categories;
        this.TaxPercentage = TaxPercentage;
    }

    public readonly static Store Instance;

    static Store()
    {
        List<Product> products = LoadProducts();
        IEnumerable<Category> categories = Categories.Instance.GetCategories();
        const taxPercentage = 13;
        

        Store.Instance = new Store(products, categories, taxPercentage);
    }

    internal static List<Product> LoadProducts()
    {
        List<Dictionary<string, string>> productData = StoreDB.RetrieveDatabaseInfo();
        List<Product> products = new List<Product>();

        foreach (var row in productData)
        {
            if (row.ContainsKey("id") && row.ContainsKey("price"))
            {
                if (int.TryParse(row["id"], out int id) &&
                    decimal.TryParse(row["price"], out decimal price) &&
                    int.TryParse(row["idCategory"], out int idCategory)) // Conversión de idCategory a int

                {
                    string name = row["name"];
                    string author = row["author"];
                    string imageUrl = row["imgUrl"];

                    Category category = Categories.Instance.GetCategories().SingleOrDefault(c => c.IdCategory == idCategory);

                    if (!category.Equals(default(Category)))
                    {
                        Product product = new Product
                        {
                            Id = id,
                            Name = name,
                            Author = author,
                            Price = price,
                            ProductCategory = category,
                            ImgUrl = imageUrl
                        };

                        products.Add(product);
                    }
                    else
                    {
                        throw new Exception($"No se encontró la categoría correspondiente al ID {idCategory}.");
                    }
                }
            }
        }

        return products;
    }


}
