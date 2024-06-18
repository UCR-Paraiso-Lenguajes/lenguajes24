using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using KEStoreApi.Data;

namespace KEStoreApi
{
    public sealed class Products
    {
        public IEnumerable<Product> ProductsStore { get; private set; }
        public Dictionary<int, List<Product>> ProductDictionary { get; private set; }
        private static readonly Lazy<Task<Products>> _instance = new Lazy<Task<Products>>(() => InitializeAsync());

        private Products(IEnumerable<Product> products)
        {
            ProductsStore = products ?? throw new ArgumentNullException(nameof(products));
            ProductDictionary = ProductsStore
                .GroupBy(p => p.CategoriaId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public static Task<Products> Instance => _instance.Value;

        private static async Task<Products> InitializeAsync()
        {
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            return new Products(products);
        }

        public static Products InitializeFromMemory(IEnumerable<Product> products)
        {
            return new Products(products);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(IEnumerable<int> categoryIds)
        {
            if (categoryIds == null) throw new ArgumentNullException(nameof(categoryIds));
            if (!categoryIds.All(id => id > 0)) throw new ArgumentException("Invalid category ID(s) provided.");

            return ProductsStore.Where(p => categoryIds.Contains(p.CategoriaId));
        }

        public async Task<List<Product>> SearchProductsByName(string productName)
        {
            if (string.IsNullOrEmpty(productName)) throw new ArgumentException("El nombre del producto no puede ser nulo o vacío.", nameof(productName));

            return ProductsStore.Where(p => p.Name.Contains(productName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<List<Product>> SearchProducts(string productName, IEnumerable<int> categoryIds)
        {
            if (string.IsNullOrEmpty(productName)) throw new ArgumentException("El nombre del producto no puede ser nulo o vacío.", nameof(productName));
            if (categoryIds == null || !categoryIds.Any()) throw new ArgumentException("Debe proporcionar al menos un ID de categoría.", nameof(categoryIds));

            return ProductsStore
                .Where(p => p.Name.Contains(productName, StringComparison.OrdinalIgnoreCase) && categoryIds.Contains(p.CategoriaId))
                .ToList();
        }
    }
}
