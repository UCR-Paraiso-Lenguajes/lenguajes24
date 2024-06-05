using MySqlConnector;
using System;
using storeApi.Models;
using System.Net.Http.Headers;
using storeApi.db;

namespace storeApi
{
    public sealed class Store
    {
        public IEnumerable<Product> Products { get; private set; }
        public Dictionary<int, List<Product>> CategoriesProducts { get; private set; }
        public IEnumerable<Category.ProductCategory> CategoriesNames { get; private set; }
        public const int TaxPercent = 13;

        private Store(IEnumerable<Product> products)
        {
            this.Products = products;
        }
        public readonly static Store Instance;
        static Store()
        {
            Task<IEnumerable<Product>> productsTask = productsFromDB();
            Store.Instance = new Store(productsTask.Result);
            var category = new Category();
            Store.Instance.CategoriesNames = category.GetCategoryNames();
        }

        private static async Task<IEnumerable<Product>> productsFromDB()
        {
            return await StoreDB.GetProductsAsync();
        }

        private void RelateProductsToCategories()
        {
            if (Products.Count() <= 0) throw new ArgumentException("Products are missing for category relation. Store");

            this.CategoriesProducts = new Dictionary<int, List<Product>>();

            foreach (Product product in Products)
            {
                if (!this.CategoriesProducts.ContainsKey(product.Category.Id))
                {
                    this.CategoriesProducts[product.Category.Id] = new List<Product>();
                }
                this.CategoriesProducts[product.Category.Id].Add(product);
            }
        }

        public async Task<Store> GetFilteredProductsAsync(IEnumerable<int> categoryIds)
        {
            await Task.Run(() => Store.Instance.RelateProductsToCategories());
            List<Product> products = new List<Product>();

            if (categoryIds == null || !categoryIds.Any())
            {
                foreach (var categoryProducts in CategoriesProducts.Values)
                {
                    products.AddRange(categoryProducts);
                }
            }
            else
            {
                foreach (var categoryId in categoryIds)
                {
                    if (CategoriesProducts != null && CategoriesProducts.ContainsKey(categoryId))
                    {
                        products.AddRange(CategoriesProducts[categoryId]);
                    }
                    else
                    {
                        throw new ArgumentException("No products found for the specified category.");
                    }
                }
            }

            return new Store(products);
        }

        private List<Product> BinarySearchProducts(List<Product> sortedProducts, string searchText, Func<Product, string> selector)
        {
            int left = 0;
            int right = sortedProducts.Count - 1;
            List<Product> foundProducts = new List<Product>();
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int compareResult = selector(sortedProducts[mid]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
                if (compareResult >= 0)
                {
                    int leftIndex = mid;
                    int rightIndex = mid;
                    while (leftIndex >= 0 && selector(sortedProducts[leftIndex]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (!foundProducts.Contains(sortedProducts[leftIndex]))
                        {
                            foundProducts.Add(sortedProducts[leftIndex]);
                        }
                        leftIndex--;
                    }
                    while (rightIndex < sortedProducts.Count && selector(sortedProducts[rightIndex]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (!foundProducts.Contains(sortedProducts[rightIndex]))
                        {
                            foundProducts.Add(sortedProducts[rightIndex]);
                        }
                        rightIndex++;
                    }
                    break;
                }
                if (string.Compare(selector(sortedProducts[mid]), searchText, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return foundProducts;
        }

        public List<Product> GetFilteredTextProducts(string searchText)
        {
            var sortedProducts = Products.OrderBy(p => p.Name).ToList();
            var nameResults = BinarySearchProducts(sortedProducts, searchText, p => p.Name);

            sortedProducts = Products.OrderBy(p => p.Description).ToList();
            var descriptionResults = BinarySearchProducts(sortedProducts, searchText, p => p.Description);

            return nameResults.Union(descriptionResults).ToList();
        }
    }
}