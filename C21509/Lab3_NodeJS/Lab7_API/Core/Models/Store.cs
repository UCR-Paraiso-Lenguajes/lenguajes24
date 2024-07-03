using Core.Models;
using Store_API.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store_API.Models
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; } = 13;

        public static readonly Store Instance;

        static Store()
        {
            DB_API dbApi = new DB_API();
            dbApi.ConnectDB();

            List<Product> dbProducts = dbApi.SelectProducts();
            Instance = new Store(dbProducts);
        }

        private Store(List<Product> products)
        {
            this.Products = products;
        }

        public void AddNewProductToStore(Product newProduct)
        {
            Products.Add(newProduct);
        }

        public async Task<Product> GetProductByNameAndCategoryIdAsync(string productName, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("El nombre del producto no puede ser nulo o vacío.", nameof(productName));
            }

            if (categoryId <= 0)
            {
                throw new ArgumentException("El ID de la categoría debe ser un valor positivo.", nameof(categoryId));
            }

            var products = Store.Instance.Products.OrderBy(p => p.Name).ThenBy(p => p.Categoria.IdCategory).ToList();
            var result = await Task.Run(() => BinarySearch(products, productName, categoryId));

            if (result == null)
            {
                throw new KeyNotFoundException($"Producto con nombre '{productName}' y ID de categoría '{categoryId}' no encontrado.");
            }

            return result;
        }

        private Product BinarySearch(List<Product> products, string productName, int categoryId)
        {
            int left = 0;
            int right = products.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var currentProduct = products[mid];
                int compareResult = CompareProducts(currentProduct, productName, categoryId);

                if (compareResult == 0)
                {
                    return currentProduct;
                }
                else if (compareResult < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return null;
        }

        private int CompareProducts(Product product, string productName, int categoryId)
        {
            int nameComparison = string.Compare(product.Name, productName, StringComparison.OrdinalIgnoreCase);
            if (nameComparison == 0)
            {
                return product.Categoria.IdCategory.CompareTo(categoryId);
            }
            return nameComparison;
        }
    }
}