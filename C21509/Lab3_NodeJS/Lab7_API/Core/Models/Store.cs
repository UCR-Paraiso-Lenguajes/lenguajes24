using Core.Models;
using Store_API.Database;

namespace Store_API.Models
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; } = 13;

        private Store(List<Product> products)
        {
            this.Products = products;
        }

        public readonly static Store Instance;

        // Static constructor
        static Store()
        {
            DB_API dbApi = new DB_API();

            var products = new List<Product>
            {
                new Product
                {
                    Id= 1,
                    Name = $"Iphone",
                    ImageURL = $"/img/Iphone.jpg",
                    Price = 200M,
                    Categoria  = new Category(1, "Electrónica")
                },
                new Product
                {
                    Id= 2,
                    Name = $"Audifono",
                    ImageURL = $"/img/audifonos.jpg",
                    Price = 100M,
                    Categoria  = new Category(1, "Electrónica")
                },
                new Product
                {
                    Id= 3,
                    Name = $"Mouse",
                    ImageURL = $"/img/mouse.jpg",
                    Price = 35M,
                    Categoria  = new Category(2, "Hogar y oficina")
                },
                new Product
                {
                    Id= 4,
                    Name = $"Pantalla",
                    ImageURL = $"/img/Pantalla.jpg",
                    Price = 68M,
                    Categoria  = new Category(3, "Entretenimiento")
                },
                new Product
                {
                    Id= 5,
                    Name = $"Headphone",
                    ImageURL = $"/img/Headphone.jpg",
                    Price = 35M,
                    Categoria  = new Category(3, "Entretenimiento")
                },
                new Product
                {
                    Id= 6,
                    Name = $"Teclado",
                    ImageURL = $"/img/teclado.jpg",
                    Price = 95M,
                    Categoria  = new Category(1, "Electrónica")
                },
                new Product
                {
                    Id= 7,
                    Name = $"Cable USB",
                    ImageURL = $"/img/Cable.jpg",
                    Price = 10M,
                    Categoria  = new Category(4, "Tecnología")
                },
                new Product
                {
                    Id= 8,
                    Name = $"Chromecast",
                    ImageURL = $"/img/Chromecast.jpg",
                    Price = 150M,
                    Categoria  = new Category(4, "Tecnología")
                }
            };

          

            List<Product> dbProducts = dbApi.SelectProducts();

            Store.Instance = new Store(dbProducts);
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

                Console.WriteLine($"Searching: {currentProduct.Name}, {currentProduct.Categoria.IdCategory}");

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
            // Compara los nombres de los productos de manera insensible a mayúsculas y minúsculas
            int nameComparison = string.Compare(product.Name, productName, StringComparison.OrdinalIgnoreCase);
            if (nameComparison == 0)
            {
                return product.Categoria.IdCategory.CompareTo(categoryId);
            }
            return nameComparison;
        }

    }
}