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
        private readonly Products _productsInstance;
        public IEnumerable<Product> ProductsList { get; private set; }
        public int TaxPercentage { get; private set; }
        public IEnumerable<Categoria> CategoriasLista { get; private set; }

        private Store(IEnumerable<Product> products, int taxPercentage, IEnumerable<Categoria> categorias)
        {
            bool areProductsInvalid = products == null || !products.Any();
            if (areProductsInvalid)
                throw new ArgumentException("La lista de productos no puede ser nula ni estar vacía.", nameof(products));

            bool isTaxPercentageInvalid = taxPercentage < 0 || taxPercentage > 13;
            if (isTaxPercentageInvalid)
                throw new ArgumentOutOfRangeException(nameof(taxPercentage), "El porcentaje de impuestos no puede ser mayor a 13%");

            bool areCategoriesInvalid = categorias == null || !categorias.Any();
            if (areCategoriesInvalid)
                throw new ArgumentException($"La lista de {nameof(categorias)} no puede ser nula ni estar vacía");

            this.ProductsList = products;
            this.TaxPercentage = taxPercentage;
            this.CategoriasLista = categorias;
            this._productsInstance = Products.InitializeFromMemory(products);
        }

        public static async Task<Store> InitializeInstanceAsync()
        {
            var categorias = Categorias.Instance.GetCategorias();
            var productsInstance = await Products.Instance;
            return new Store(productsInstance.ProductsStore, 13, categorias);
        }

        public async Task<IEnumerable<Product>> getProductosCategoryID(IEnumerable<int> categoryIds)
        {
            if (categoryIds == null)
                throw new ArgumentNullException(nameof(categoryIds), "Los IDs de categoría no pueden ser nulos.");
            return await _productsInstance.GetProductsByCategory(categoryIds);
        }

        public async Task<List<Product>> searchProductsStore(string productName, IEnumerable<int> categoryIds)
        {
            if (string.IsNullOrEmpty(productName))
                throw new ArgumentException("El nombre del producto no puede ser nulo o vacío.", nameof(productName));

            if (categoryIds == null || !categoryIds.Any())
            {
                return await _productsInstance.SearchProductsByName(productName);
            }
            else
            {
                return await _productsInstance.SearchProducts(productName, categoryIds);
            }
        }

        private static readonly Lazy<Task<Store>> InstanceTask = new Lazy<Task<Store>>(InitializeInstanceAsync);
        public static Task<Store> Instance => InstanceTask.Value;
    }
}
