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
            if (products == null)
                throw new ArgumentNullException(nameof(products), "La lista de productos no puede ser nula.");
            if (!products.Any())
                throw new ArgumentException("La lista de productos debe contener al menos un elemento.", nameof(products));

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

        public static async Task<Products> InitializeAsync()
        {
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            return new Products(products);
        }

        public static async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            if (categoryId < 1)
                throw new ArgumentException("El ID de categoría debe ser mayor o igual a 1.", nameof(categoryId));

            var products = await InitializeAsync();

            if (products.ProductDictionary.TryGetValue(categoryId, out List<Product> productList))
                return productList;

            return new List<Product>();
        }

        private static readonly Lazy<Task<Products>> InstanceTask = new Lazy<Task<Products>>(InitializeAsync);
        public static Task<Products> Instance => InstanceTask.Value;
    }
}
