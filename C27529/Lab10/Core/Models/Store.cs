using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeApi.Models;
using storeApi.db;
using storeApi.Business;

namespace storeApi
{
    public sealed class Store
    {
        public IEnumerable<PaymentMethod> PaymentMethods { get; private set; }
        public IEnumerable<Product> Products { get; private set; }
        public Dictionary<int, List<Product>> CategoriesProducts { get; private set; }
        public IEnumerable<Category.ProductCategory> CategoriesNames { get; private set; }
        public const int TaxPercent = 13;

        public Store(IEnumerable<Product> products)
        {
            this.Products = products ?? throw new ArgumentNullException(nameof(products));
            this.RelateProductsToCategories();
            var category = new Category();
            this.CategoriesNames = category.GetCategoryNames();
            this.PaymentMethods = new List<PaymentMethod> { new Cash(), new Sinpe() };

        }


        // Constructor adicional para pruebas
        public Store(IEnumerable<Product> products, bool skipStaticInitialization)
        {
            this.Products = products ?? throw new ArgumentNullException(nameof(products));
            this.RelateProductsToCategories();
            var category = new Category();
            this.CategoriesNames = category.GetCategoryNames();
            if (!skipStaticInitialization) { }
        }


        public static Store Instance { get; private set; }

        static Store()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Testing")
            {
                try
                {
                    var productsTask = productsFromDB().GetAwaiter().GetResult();
                    Store.Instance = new Store(productsTask);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error during static initialization: {ex.Message}");
                }
            }
        }


        
        public void DisablePaymentMethod(PaymentMethod.Type paymentType)
        {
            PaymentMethods = PaymentMethods.Where(pm => pm.PaymentType != paymentType).ToList();
        }

        private static async Task<IEnumerable<Product>> productsFromDB()
        {
            try
            {
                return await StoreDB.GetProductsAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error retrieving products from DB: {ex.Message}");

            }
        }

        private void RelateProductsToCategories()
        {
            if (!Products.Any()) throw new ArgumentException("Products are missing for category relation. Store");

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
            await Task.Run(() => this.RelateProductsToCategories());
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

        public List<Product> GetFilteredTextProducts(string searchText)
        {
            var sortedProducts = Products.OrderBy(p => p.Name).ToList();
            var nameResults = BinarySearchProducts(sortedProducts, searchText, p => p.Name);

            sortedProducts = Products.OrderBy(p => p.Description).ToList();
            var descriptionResults = BinarySearchProducts(sortedProducts, searchText, p => p.Description);

            return nameResults.Union(descriptionResults).ToList();
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

        public void AddProductToStore(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            if (string.IsNullOrEmpty(product.Description)) throw new ArgumentNullException(nameof(product.Description), "Description cannot be null.");
            if (string.IsNullOrEmpty(product.ImageURL)) throw new ArgumentNullException(nameof(product.ImageURL), "An image is needed.");
            if (string.IsNullOrEmpty(product.Name)) throw new ArgumentNullException(nameof(product.Name), "The product must have a name.");
            if (product.Price < 0) throw new ArgumentException("The price cannot be less than 0.");

            var targetInstance = Instance ?? this;
            var productsList = targetInstance.Products.ToList();
            productsList.Add(product);
            targetInstance.Products = productsList;
            targetInstance.RelateProductsToCategories();
        }


    }
}
