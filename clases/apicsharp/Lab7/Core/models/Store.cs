using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace TodoApi.models;
public sealed class Store
{
    public List<Product> Products { get; private set; }
    public int TaxPercentage { get; private set; }
    private Store( List<Product> products, int TaxPercentage )
    {
        this.Products = products;
        this.TaxPercentage = TaxPercentage;
    }

    public readonly static Store Instance;
    // Static constructor
    static Store()
    {
        var products = new List<Product>();

        // Generate 30 sample products
        for (int i = 1; i <= 30; i++)
        {
            products.Add(new Product
            {
                Name = $"Product {i}",
                ImageUrl = $"https://example.com/image{i}.jpg",
                Price = 10.99m * i,
                Description = $"Description of Product {i}",
                Uuid = Guid.NewGuid()
            });
        }

        Store.Instance = new Store(products, 13);

        }
}