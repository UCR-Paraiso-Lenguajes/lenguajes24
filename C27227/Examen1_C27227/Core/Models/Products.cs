using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KEStoreApi;
using KEStoreApi.Data;

namespace Core
{
    public class Products
    {
        public IEnumerable<Product> ProductsStore { get; private set; }
        public Dictionary<int, List<Product>> ProductDictionary { get; private set; }

        private Products(IEnumerable<Product> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products), "La lista de productos no puede ser nula.");
            if (!products.Any()) throw new ArgumentException("La lista de productos debe contener al menos un elemento.", nameof(products));
            ProductsStore = products;
            ProductDictionary = new Dictionary<int, List<Product>>();
            foreach (var product in products)
            {
                if (!ProductDictionary.TryGetValue(product.Categoria.Id, out List<Product> productList))
                {
                    productList = new List<Product>();
                    ProductDictionary[product.Categoria.Id] = productList;
                }
                productList.Add(product);
            }
        }

        public static Products InitializeFromMemory(IEnumerable<Product> products)
        {
            return new Products(products);
        }

        public static async Task<Products> InitializeAsync()
        {
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            return new Products(products);
        }

        public async Task<List<Product>> GetProductsByCategory(IEnumerable<int> categoryIds)
        {
            if (categoryIds == null)
                throw new ArgumentNullException(nameof(categoryIds), "Los IDs de categoría no pueden ser nulos.");

            var validCategoryIds = categoryIds.Where(id => id >= 1).ToList();

            if (!validCategoryIds.Any())
                throw new ArgumentException("Se debe proporcionar al menos un ID de categoría válido (mayor o igual a 1).", nameof(categoryIds));

            List<Product> productsList = new List<Product>();

            foreach (int categoryId in validCategoryIds)
            {
                if (ProductDictionary.TryGetValue(categoryId, out List<Product> productList))
                    productsList.AddRange(productList);
            }

            return await Task.FromResult(productsList);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await Task.FromResult(ProductsStore.ToList());
        }

        public async Task<List<Product>> SearchProductsByName(string productName)
        {
            var products = await GetAllProducts();

            var bst = new BinarySearchTree<Product>();
            foreach (var product in products)
            {
                bst.Insert(product);
            }

            return bst.Search(productName);
        }

        public async Task<List<Product>> SearchProducts(string productName, IEnumerable<int> categoryIds)
        {
            var products = await GetProductsByCategory(categoryIds);

            var bst = new BinarySearchTree<Product>();
            foreach (var product in products)
            {
                bst.Insert(product);
            }

            return bst.Search(productName);
        }

        private static readonly Lazy<Task<Products>> InstanceTask = new Lazy<Task<Products>>(InitializeAsync);
        public static Task<Products> Instance => InstanceTask.Value;
    }
}
