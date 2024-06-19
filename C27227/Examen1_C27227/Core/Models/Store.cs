using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using KEStoreApi.Data;

namespace KEStoreApi
{
    public sealed class Store
    {
        private Products _productsInstance;
        public IEnumerable<Product> ProductsList { get; private set; }
        public int TaxPercentage { get; private set; }
        public IEnumerable<Categoria> CategoriasLista { get; private set; }
        public delegate Task ProductActionDelegate(Product product);

        private Store(IEnumerable<Product> products, int taxPercentage, IEnumerable<Categoria> categorias)
        {
            if (products == null || !products.Any())
                throw new ArgumentException("La lista de productos no puede ser nula ni estar vacía.", nameof(products));

            if (taxPercentage < 0 || taxPercentage > 13)
                throw new ArgumentOutOfRangeException(nameof(taxPercentage), "El porcentaje de impuestos no puede ser mayor a 13%");

            if (categorias == null || !categorias.Any())
                throw new ArgumentException($"La lista de {nameof(categorias)} no puede ser nula ni estar vacía");

            this.ProductsList = products.Where(p => !p.IsDeleted);
            this.TaxPercentage = taxPercentage;
            this.CategoriasLista = categorias;
            this._productsInstance = Products.InitializeFromMemory(this.ProductsList);
        }

        public static async Task<Store> InitializeInstanceAsync()
        {
            var categorias = Categorias.Instance.GetCategorias();
            var productsInstance = await Products.Instance;
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            return new Store(products, 13, categorias);
        }

        public async Task<IEnumerable<Product>> GetProductosCategoryIDAsync(IEnumerable<int> categoryIds)
        {
            if (categoryIds == null)
                throw new ArgumentNullException(nameof(categoryIds), "Los IDs de categoría no pueden ser nulos.");
            await RefreshProductsListAsync();
            return _productsInstance.ProductsStore.Where(p => categoryIds.Contains(p.CategoriaId) && !p.IsDeleted);
        }

        public async Task<List<Product>> SearchProductsStoreAsync(string productName, IEnumerable<int> categoryIds)
        {
            if (string.IsNullOrEmpty(productName))
                throw new ArgumentException("El nombre del producto no puede ser nulo o vacío.", nameof(productName));

            await RefreshProductsListAsync();

            if (categoryIds == null || !categoryIds.Any())
            {
                return _productsInstance.ProductsStore.Where(p => p.Name.Contains(productName, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted).ToList();
            }
            else
            {
                return _productsInstance.ProductsStore.Where(p => p.Name.Contains(productName, StringComparison.OrdinalIgnoreCase) && categoryIds.Contains(p.CategoriaId) && !p.IsDeleted).ToList();
            }
        }

        public async Task AddProductAsync(Product product)
        {
            ProductActionDelegate actions = async (newProduct) =>
            {
                var productsList = ProductsList.ToList();
                productsList.Add(newProduct);
                ProductsList = productsList.Where(p => !p.IsDeleted);
                _productsInstance = Products.InitializeFromMemory(ProductsList);
                await Task.CompletedTask;
            };

            await DatabaseStore.AddProductAsync(product, actions);
        }

        public async Task<object> GetAllProductsAndCategoriesAsync()
        {
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            var categorias = Categorias.Instance.GetCategorias();

            var productsWithCategory = products.Where(p => !p.IsDeleted).Select(product =>
            {
                var categoria = categorias.FirstOrDefault(c => c.Id == product.CategoriaId);
                return new
                {
                    product.Id,
                    product.Name,
                    product.Price,
                    product.ImageUrl,
                    product.Quantity,
                    product.CategoriaId,
                    CategoriaNombre = categoria.Equals(default(Categoria)) ? "Sin categoría" : categoria.Nombre
                };
            });

            return new
            {
                Products = productsWithCategory,
                Categorias = categorias
            };
        }

        public async Task RefreshProductsListAsync()
        {
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            ProductsList = products.Where(p => !p.IsDeleted);
            _productsInstance = Products.InitializeFromMemory(ProductsList);
        }

        private static readonly Lazy<Task<Store>> InstanceTask = new Lazy<Task<Store>>(InitializeInstanceAsync);
        public static Task<Store> Instance => InstanceTask.Value;
    }
}
