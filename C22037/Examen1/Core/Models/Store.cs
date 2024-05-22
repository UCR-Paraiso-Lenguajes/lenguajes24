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

        private Store(IEnumerable<Product> products, int taxPercentage)
        {
            if (products == null) throw new ArgumentNullException(nameof(products), "Products cannot be null.");
            if (!products.Any()) throw new ArgumentException("Products list cannot be empty.", nameof(products));
            if (taxPercentage < 0 || taxPercentage > 100) throw new ArgumentOutOfRangeException(nameof(taxPercentage), "Tax percentage must be between 0 and 100.");

            Products = products;
            TaxPercentage = taxPercentage;
        }

        public static async Task<Store> InstanceAsync()
        {
            var products = await StoreDB.GetProductsAsync();
            var categories = new Categories().GetCategories();
            var store = new Store(products, 13);
            store.categoryLogic = new CategoryLogic(categories, products);
            return store;
        }

        public async Task<Store> GetProductsByCategoryAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Category ID must be greater than 0.");

            var productsByCategory = await categoryLogic.GetCategoriesByIdAsync(new[] { id });
            return new Store(productsByCategory, 13);
        }

    }
}