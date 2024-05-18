using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Business
{
    public sealed class CategoryLogic
    {
        private static Dictionary<int, List<Product>> productsByCategoryId;

        public CategoryLogic(Categories.CategorySt[] categories, IEnumerable<Product> products)
        {
            if (categories == null || categories.Length == 0) throw new ArgumentException("The list of categories cannot be null or empty.");
            if (products == null) throw new ArgumentNullException("The list of products cannot be null.");
            if (products.Any(p => p == null)) throw new ArgumentException("The list of products cannot contain null elements.");

            productsByCategoryId = new Dictionary<int, List<Product>>();

            foreach (var category in categories)
            {
                productsByCategoryId.Add(category.Id, new List<Product>());
            }

            foreach (var product in products)
            {
                if (productsByCategoryId.TryGetValue(product.Category.Id, out var productList))
                {
                    productList.Add(product);
                }
            }

            foreach (var key in productsByCategoryId.Keys.ToList())
            {
                productsByCategoryId[key] = productsByCategoryId[key].OrderBy(p => p.Name).ToList();
            }
        }

        public async Task<IEnumerable<Product>> GetCategoriesByIdAsync(IEnumerable<int> categoryIds)
        {
            if (categoryIds == null) throw new ArgumentNullException("Categories cannot be null");

            List<Product> products = new List<Product>();

            foreach (var categoryId in categoryIds)
            {
                if (productsByCategoryId.TryGetValue(categoryId, out List<Product> categoryProducts))
                {
                    products.AddRange(categoryProducts);
                }
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsBySearchAsync(string search, string categories)
        {
            if (string.IsNullOrEmpty(search)) throw new ArgumentNullException("Search term cannot be null or empty.");

            List<Product> matchingProducts = new List<Product>();
            List<int> categoryIdsList = categories?.Split(',').Select(int.Parse).ToList();

            if (categoryIdsList == null || !categoryIdsList.Any())
            {
                foreach (var id in productsByCategoryId)
                {
                    matchingProducts.AddRange(SearchProducts(id.Value, search));
                }
            }
            else
            {
                foreach (var categoryId in categoryIdsList)
                {
                    if (productsByCategoryId.TryGetValue(categoryId, out List<Product> categoryProducts))
                    {
                        matchingProducts.AddRange(SearchProducts(categoryProducts, search));
                    }
                }
            }

            return matchingProducts;
        }

        private IEnumerable<Product> SearchProducts(List<Product> products, string search)
        {
            List<Product> results = new List<Product>();
            search = search.ToLower();

            int left = 0;
            int right = products.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                string productName = products[mid].Name.ToLower();
                string productDescription = products[mid].Description.ToLower();
                string productPrice = products[mid].Price.ToString().ToLower();

                if (productName.Contains(search) || productDescription.Contains(search) || productPrice.Contains(search))
                {
                    int start = mid;
                    int end = mid;

                    while (start >= left && (products[start].Name.ToLower().Contains(search) || products[start].Description.ToLower().Contains(search) || products[start].Price.ToString().ToLower().Contains(search)))
                    {
                        results.Add(products[start]);
                        start--;
                    }

                    while (end <= right && (products[end].Name.ToLower().Contains(search) || products[end].Description.ToLower().Contains(search) || products[end].Price.ToString().ToLower().Contains(search)))
                    {
                        if (end != mid) results.Add(products[end]);
                        end++;
                    }

                    break;
                }
                else if (productName.CompareTo(search) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return results;
        }

    }
}