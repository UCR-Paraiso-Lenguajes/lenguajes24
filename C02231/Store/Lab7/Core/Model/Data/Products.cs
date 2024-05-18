using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using StoreAPI.Database;

namespace StoreAPI.models;


public class Products
{
    private IEnumerable<Product> allProducts;
    private Dictionary<int, List<Product>> productByCategoryId;

    private Dictionary<string, List<Product>> productByName;
    public Products() { }
    public Products(IEnumerable<Product> allProducts, Dictionary<int, List<Product>> productByCategoryId, Dictionary<string, List<Product>> productByName)
    {
        if (allProducts == null || allProducts.Count() == 0) throw new ArgumentNullException($"The list of products {nameof(allProducts)}can't be null.");
        if (productByCategoryId == null || productByCategoryId.Count() == 0) throw new ArgumentNullException($"The {nameof(productByCategoryId)} can't be null or 0.");


        this.allProducts = allProducts;
        this.productByCategoryId = productByCategoryId;
        this.productByName = new Dictionary<string, List<Product>>();

        // Index products por name
        foreach (var product in allProducts.OrderBy(p => p.Name))
        {
            if (!productByName.ContainsKey(product.Name))
            {
                productByName[product.Name] = new List<Product>();
            }
            productByName[product.Name].Add(product);
        }
    }
    public static Products Instance;


    static async Task<Products> InitializeAsync()
    {
        var products = await Store.LoadProductsAsync();
        Dictionary<int, List<Product>> productsByCategoryId = new Dictionary<int, List<Product>>();
        Dictionary<string, List<Product>> productsByName = new Dictionary<string, List<Product>>();

        foreach (var product in products)
        {
            if (!productsByCategoryId.TryGetValue(product.ProductCategory.IdCategory, out var productCategoryId))
            {
                productCategoryId = new List<Product>();
                productsByCategoryId[product.ProductCategory.IdCategory] = productCategoryId;
            }
            productCategoryId.Add(product);

            if (!productsByName.TryGetValue(product.Name, out var nameProducts))
            {
                nameProducts = new List<Product>();
                productsByName[product.Name] = nameProducts;
            }
            nameProducts.Add(product);
        }

        return new Products(products, productsByCategoryId, productsByName);
    }

    public static async Task InitializeInstanceAsync()
    {
        Instance = await InitializeAsync();
    }


    public async Task<IEnumerable<Product>> GetProductsCategoryAsync(IEnumerable<int> categoryIds)
    {
        if (categoryIds == null || !categoryIds.Any()) throw new ArgumentException($"The list of {nameof(categoryIds)} can't be null or empty.");

        List<Product> products = new List<Product>();

        foreach (var categoryId in categoryIds)
        {
            if (categoryId < 0) throw new ArgumentException($"The {nameof(categoryId)} number must be greater than 0");

            if (categoryId == 0) products.AddRange(allProducts);

            if (productByCategoryId.TryGetValue(categoryId, out List<Product> categoryProducts))
            {
                products.AddRange(categoryProducts);
            }
            else
            {
                throw new ArgumentException($"The category ID {categoryId} does not exist.");
            }
        }
        return await Task.FromResult(products);
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be null or empty.");

        if (productByName.TryGetValue(productName, out List<Product> products))
        {
            return await Task.FromResult(products);
        }
        else
        {
            throw new ArgumentException($"No products found with name '{productName}'.");
        }
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(IEnumerable<int> categoryIds, string keywords)
    {
        if (categoryIds == null) throw new ArgumentNullException($"The parameter {nameof(categoryIds)} cannot be null.");
        if (categoryIds.Any(id => id < 0)) throw new ArgumentException($"Invalid category IDs are provided.");

        if (keywords == null) throw new ArgumentNullException($"The parameter {nameof(keywords)} cannot be null.");


        List<Product> matchingProducts = new List<Product>();
        if (productByCategoryId == null)
            throw new InvalidOperationException("Product dictionary by category is null.");

        // Si categoryIds contiene 0, buscar en todas las categorías
        if (categoryIds.Contains(0))
        {
            foreach (var productList in productByCategoryId.Values)
            {
                foreach (var product in productList)
                {
                    if (ProductMatchesKeywords(product, keywords))
                    {
                        matchingProducts.Add(product);
                    }
                }
            }
        }
        else if (categoryIds != null && categoryIds.Any())
        {
            foreach (var categoryId in categoryIds)
            {
                if (productByCategoryId.TryGetValue(categoryId, out List<Product> categoryProducts))
                {
                    foreach (var product in categoryProducts)
                    {
                        if (ProductMatchesKeywords(product, keywords))
                        {
                            matchingProducts.Add(product);
                        }
                    }
                }
            }

        }
        else // Buscar en todos los productos si no se indica ninguna categoría
        {
            foreach (var productList in productByCategoryId.Values)
            {
                foreach (var product in productList)
                {
                    if (ProductMatchesKeywords(product, keywords))
                    {
                        matchingProducts.Add(product);
                    }
                }
            }
        }

        return await Task.FromResult(matchingProducts);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string keywords)
    {
        List<Product> matchingProducts = new List<Product>();

        foreach (var productList in productByCategoryId.Values)
        {
            foreach (var product in productList)
            {
                if (ProductMatchesKeywords(product, keywords))
                {
                    matchingProducts.Add(product);
                }
            }
        }

        return await Task.FromResult(matchingProducts);
    }

    private bool ProductMatchesKeywords(Product product, string keywords)
    {
        if (string.IsNullOrWhiteSpace(keywords))
        {
            return true; // Si no hay palabras clave, consideramos que todos los productos coinciden.
        }
        // Compruebe si algún campo del producto contiene las palabras clave
        return product.Name.Contains(keywords, StringComparison.OrdinalIgnoreCase) ||
               product.Author.Contains(keywords, StringComparison.OrdinalIgnoreCase) ||
               product.ProductCategory.Name.Contains(keywords, StringComparison.OrdinalIgnoreCase);
    }

}
