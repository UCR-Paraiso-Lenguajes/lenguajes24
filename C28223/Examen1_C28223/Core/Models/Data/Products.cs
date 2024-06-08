using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using storeApi.DataBase;

namespace storeApi.Models.Data
{
    public class Products
    {
        private IEnumerable<Product> allProducts;
        private Dictionary<int, List<Product>> productsByCategory;
        private IEnumerable<Category> Category;
        private BinarySearchTree TreeSearch;
        public Products() { }
        private Products(IEnumerable<Product> allProducts, Dictionary<int, List<Product>> productsByCategory, IEnumerable<Category> categories)
        {
            if (allProducts == null || allProducts.Count() == 0) throw new ArgumentNullException($"La lista de productos {nameof(allProducts)} no puede ser nula.");
            if (productsByCategory == null || productsByCategory.Count() == 0) throw new ArgumentNullException($"El diccionario {nameof(productsByCategory)}de productos por categoría no puede ser nulo.");
            if (categories == null || categories.Count() == 0) throw new ArgumentNullException($"La lista de categorías {nameof(categories)} no puede ser nula.");
            this.allProducts = allProducts;
            this.productsByCategory = productsByCategory;
            this.Category = categories;
            this.TreeSearch = new BinarySearchTree();
            foreach (var product in allProducts)
            {
                TreeSearch.InsertProduct(product);
            }

        }
        public async Task<Products> GetInstanceAsync()
        {
            var storedb = new StoreDataBase();
            if (storedb == null) throw new InvalidOperationException("No se pudo acceder a la base de datos de la tienda.");
            var categories = new Categories();
            var categoriesList = categories.GetCategories();
            if (categoriesList == null || !categoriesList.Any()) throw new InvalidOperationException("No se pudieron obtener las categorías de productos.");
            var products = await storedb.GetProductsFromDBAsync();
            if (products == null || !products.Any()) throw new InvalidOperationException("No se pudieron obtener los productos de la base de datos.");
            var productsByCategory = GroupProductsByCategory(products);
            if (productsByCategory == null || !productsByCategory.Any()) throw new InvalidOperationException("No se pudieron agrupar los productos por categoría.");
            return new Products(products, productsByCategory, categories.GetCategories());
        }

        private Dictionary<int, List<Product>> GroupProductsByCategory(IEnumerable<Product> products)
        {
            bool esListaDeProductosNula = products == null;
            bool esListaDeProductosVacia = !esListaDeProductosNula && !products.ToList().Any();
            bool esEntradaInvalida = esListaDeProductosNula || esListaDeProductosVacia;
            if (esEntradaInvalida) throw new ArgumentException($"La lista de productos {nameof(products)} no puede ser null o estar vacia.");
            var productsByCategory = new Dictionary<int, List<Product>>();
            foreach (var p in products)
            {
                List<Product> temporal;
                if (!productsByCategory.TryGetValue(p.category.CategoryID, out temporal))
                {
                    temporal = new List<Product>();
                    productsByCategory[p.category.CategoryID] = temporal;
                }
                temporal.Add(p);
            }
            return productsByCategory;
        }

        public IEnumerable<Product> GetAllProducts() { return allProducts; }
        public IEnumerable<Category> GetCategory() { return Category; }
        public IEnumerable<Product> GetProductsByCategoryIDs(List<int> categoryIds)
        {
            var esCategoriasNula = categoryIds == null;
            var noHayCategorias = !categoryIds.Any();
            if (esCategoriasNula || noHayCategorias) throw new ArgumentException("La lista de IDs de categorías no puede estar vacía o ser nula.");
            foreach (var categoryId in categoryIds)
            {
                if (categoryId < 1) throw new ArgumentException($"El ID de la categoría {categoryId} no puede ser negativo o cero.");
            }
            var filteredProducts = new List<Product>();
            foreach (var categoryId in categoryIds)
            {
                if (productsByCategory.TryGetValue(categoryId, out var productsFound))
                {
                    filteredProducts.AddRange(productsFound);
                }
            }
            return filteredProducts;
        }

        public IEnumerable<Product> SearchByText(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) throw new ArgumentException($"El texto de búsqueda {nameof(searchText)} no puede estar vacío o ser nulo.");
            return TreeSearch.Search(searchText);//busca en todos los productos
        }

        public IEnumerable<Product> SearchByTextAndCategory(string searchText, List<int> categoryIds)
        {
            bool esTextoBusquedaNuloOVacio = string.IsNullOrWhiteSpace(searchText);
            bool sonIdsCategoriasNulos = categoryIds == null;
            bool sonIdsCategoriasVacios = !sonIdsCategoriasNulos && !categoryIds.Any();
            bool esEntradaInvalida = esTextoBusquedaNuloOVacio || sonIdsCategoriasNulos || sonIdsCategoriasVacios;
            if (esEntradaInvalida)throw new ArgumentException("El texto de búsqueda no puede estar vacío o ser nulo, y la lista de IDs de categorías no puede estar vacía o ser nula.");
            
            foreach (var categoryId in categoryIds)
            {
                if (categoryId < 1) throw new ArgumentException($"El ID de la categoría {categoryId} no puede ser negativo o cero.");
            }
            var filteredProducts = new List<Product>();
            foreach (var categoryId in categoryIds)
            {
                if (productsByCategory.TryGetValue(categoryId, out var productsFound))
                {
                    filteredProducts.AddRange(productsFound);
                }
            }
            if (filteredProducts == null || !filteredProducts.Any())
            {
                return filteredProducts;
            }
            var treeByCategories = new BinarySearchTree();
            foreach (var product in filteredProducts)
            {
                treeByCategories.InsertProduct(product);
            }
            var productsFilter = treeByCategories.Search(searchText);
            return productsFilter;
        }
    }
}