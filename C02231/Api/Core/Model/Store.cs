using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using StoreAPI.Database;

namespace StoreAPI.models;

public sealed class Store
{
    public List<Product> Products { get; private set; }
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

    public static async Task<Store> InitializeAsync()
    {
        List<Product> products = await LoadProductsAsync();
        IEnumerable<Category> categories = Categories.Instance.GetCategories();
        const int taxPercentage = 13;

        return new Store(products, categories, taxPercentage);
    }

    public readonly static Lazy<Task<Store>> Instance = new Lazy<Task<Store>>(InitializeAsync);


    public static async Task<List<Product>> LoadProductsAsync()
    {
        List<Dictionary<string, string>> productData = await StoreDB.RetrieveDatabaseInfoAsync();
        List<Product> products = new List<Product>();

        foreach (var row in productData)
        {
            if (row.ContainsKey("id") && row.ContainsKey("price"))
            {
                if (int.TryParse(row["id"], out int id) &&
                    decimal.TryParse(row["price"], out decimal price) &&
                    int.TryParse(row["idCategory"], out int idCategory))
                {
                    string name = row["name"];
                    string description = row["description"];
                    string imageUrl = row["imgUrl"];

                    Category category = Categories.Instance.GetCategories().SingleOrDefault(c => c.IdCategory == idCategory);

                    if (!category.Equals(default(Category)))
                    {
                        Product product = new Product
                        (
                            name,
                            description, 
                            imageUrl,
                            price,
                            category,
                            id
                        );

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
