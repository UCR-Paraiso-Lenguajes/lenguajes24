using MySqlConnector;
using System;
using storeApi.Models;
using System.Net.Http.Headers;
using storeApi.db;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace storeApi
{

    public sealed class Store
    {
        public IEnumerable<Product> Products { get; private set; }
        public Dictionary<int, List<Product>> CategoriesProducts { get; private set; }
        public List<Category.ProductCategory> CategoriesNames { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(IEnumerable<Product> products, int taxPercentage)
        {
            this.Products = products;
            this.TaxPercentage = taxPercentage;
            
        }



        public readonly static Store Instance;

        // Static constructor
        static Store()
        {
            Task<IEnumerable<Product>> productsTask = productsFromDB();
            Store.Instance = new Store(productsTask.Result, 13);
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
                if (!this.CategoriesProducts.ContainsKey(product.Category))
                {
                    this.CategoriesProducts[product.Category] = new List<Product>();
                }
                this.CategoriesProducts[product.Category].Add(product);
            }

        }


        public async Task<Store> GetFilteredProductsAsync(int category)
        {
            if(category<0) throw new ArgumentException("Products are missing for category relation. Please ensure category value is greater than 0.", nameof(category));
            await Task.Run(() => Store.Instance.RelateProductsToCategories());

            if (CategoriesProducts.ContainsKey(category))
            {
                IEnumerable<Product> products = CategoriesProducts[category];
                return new Store(products, 13);
            }
            else
            {
                throw new ArgumentException("No products found for the specified category.");
            }
        }


    }
}
