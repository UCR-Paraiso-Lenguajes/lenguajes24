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



    public async Task<IEnumerable<Product>> SearchProductsAsync(IEnumerable<int> categoryIdsList, string searchKeywords)
    {

        if (categoryIdsList.Any(id => id < 0)) throw new ArgumentException($"Invalid category IDs are provided.");
        if (searchKeywords == null) throw new ArgumentNullException($"The parameter {nameof(searchKeywords)} cannot be null.");
        if (categoryIdsList == null) _ = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<Product> matchingProducts = new List<Product>();

        // Si categoryIds contiene 0, buscar en todas las categorías
        if (categoryIdsList.Contains(0))
        {
            foreach (var productList in productByCategoryId.Values)
            {
                matchingProducts.AddRange(BinarySearchProducts(productList, searchKeywords));
            }
        }
        else if (categoryIdsList != null && categoryIdsList.Any())
        {
            foreach (var categoryId in categoryIdsList)
            {
                if (productByCategoryId.TryGetValue(categoryId, out List<Product> categoryProducts))
                {
                    matchingProducts.AddRange(BinarySearchProducts(categoryProducts, searchKeywords));
                }
            }
        }
        else // Buscar en todos los productos si no se indica ninguna categoría
        {
            foreach (var productList in productByCategoryId.Values)
            {
                matchingProducts.AddRange(BinarySearchProducts(productList, searchKeywords));
            }
        }

        return await Task.FromResult(matchingProducts);
    }

    private List<Product> BinarySearchProducts(List<Product> products, string searchKeywords)
    {
        if (products == null || !products.Any() || string.IsNullOrEmpty(searchKeywords)) throw new ArgumentException("Products list and keywords cannot be null or empty.");

        List<Product> matchingProducts = new List<Product>();
        var sortedProducts = products.OrderBy(p => p.Name).ToList();

        int left = 0;
        int right = sortedProducts.Count - 1;
        bool foundMatch = false;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            //int comparison = string.Compare(sortedProducts[mid].Name, keywords, StringComparison.OrdinalIgnoreCase);
            bool nameContainsKeywords = sortedProducts[mid].Name.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase);
            bool authorContainsKeywords = sortedProducts[mid].Author.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase);

            // No buscamos una coincidencia exacta, sino el primer punto de inserción
            if (nameContainsKeywords || authorContainsKeywords)
            {
                foundMatch = true;
                matchingProducts.Add(sortedProducts[mid]);

                // Buscar hacia la izquierda y derecha para encontrar todos los productos coincidentes
                int temp = mid;
                while (--temp >= 0 && (sortedProducts[temp].Name.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase) || sortedProducts[temp].Author.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase)))
                {
                    matchingProducts.Add(sortedProducts[temp]);
                }

                temp = mid;
                while (++temp < sortedProducts.Count && (sortedProducts[temp].Name.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase) || sortedProducts[temp].Author.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase)))
                {
                    matchingProducts.Add(sortedProducts[temp]);
                }

                break;
            }
            else if (string.Compare(sortedProducts[mid].Name, searchKeywords, StringComparison.OrdinalIgnoreCase) < 0)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        if (!foundMatch)
        {
            foreach (var product in sortedProducts)
            {
                if (product.Name.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase) || product.Author.Contains(searchKeywords, StringComparison.OrdinalIgnoreCase))
                {
                    matchingProducts.Add(product);
                }
            }
        }

        return matchingProducts;
    }

}