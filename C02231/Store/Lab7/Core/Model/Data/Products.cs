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
    public Products() { }
    private Products(IEnumerable<Product> allProducts, Dictionary<int, List<Product>> categoryDictionary)
    {
        if (allProducts == null || allProducts.Count() == 0) throw new ArgumentNullException($"The list of products {nameof(allProducts)}can't be null.");
        if (categoryDictionary == null || categoryDictionary.Count() == 0) throw new ArgumentNullException($"The products dictionary {nameof(categoryDictionary)}by category can't be null.");
        this.allProducts = allProducts;
        this.productByCategoryId = categoryDictionary;
    }
    public static Products Instance;


    static Products()
    {

        var products = Store.LoadProducts();
        Dictionary<int, List<Product>> productsByCategoryId = new Dictionary<int, List<Product>>();
        foreach (var product in products)
        {
            if (!productsByCategoryId.TryGetValue(product.ProductCategory.IdCategory, out var categoryProducts))
            {
                categoryProducts = new List<Product>();
                productsByCategoryId[product.ProductCategory.IdCategory] = categoryProducts;
            }
            categoryProducts.Add(product);


        }

        Products.Instance = new Products(products, productsByCategoryId);

    }


    public IEnumerable<Product> GetProductsCategory(int categoryId)
    {
        try
        {
            if (categoryId < 0)
            {
                throw new ArgumentException($"The {nameof(categoryId)} number must be greater than 0");
            }

            if (categoryId == 0)
            {
                return allProducts;
            }

            if (productByCategoryId.TryGetValue(categoryId, out var products))
            {
                return products;
            }
            return null;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("The list is empty", ex);
        }
    }

}
