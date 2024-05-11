using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Business;
using TodoApi.Database;

namespace TodoApi.Models
{
    public sealed class Store
    {
        public IEnumerable<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }
        private CategoryLogic categoryLogic;
        private Category category = new Category();

        private Store(IEnumerable<Product> products, int taxPercentage)
        {
            if (products == null) throw new ArgumentNullException(nameof(products), "Products cannot be null.");
            if (!products.Any()) throw new ArgumentException("Products list cannot be empty.", nameof(products));
            if (taxPercentage < 0 || taxPercentage > 100) throw new ArgumentOutOfRangeException(nameof(taxPercentage), "Tax percentage must be between 0 and 100.");

            Products = products;
            TaxPercentage = taxPercentage;
        }

        public static Task<Store> Instance => instance.Value;

        private static readonly Lazy<Task<Store>> instance = new Lazy<Task<Store>>(InstanceAsync);

        public static async Task<Store> InstanceAsync()
        {
            var products = await StoreDB.GetProductsAsync();
            var store = new Store(products, 13);
            store.categoryLogic = new CategoryLogic(store.category.GetCategories(), products);
            return store;
        }


        public async Task<Store> GetProductsByCategory(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Category ID must be greater than 0.");
            var productsByCategory = await categoryLogic.GetCategoryByIdAsync(id);
            return new Store(productsByCategory, 13);
        }
    }
}