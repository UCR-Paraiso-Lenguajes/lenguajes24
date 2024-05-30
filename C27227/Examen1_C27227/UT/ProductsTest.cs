using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using KEStoreApi;
using KEStoreApi.Data;

namespace Tests
{
    public class ProductsTests
    {
        private List<Product> _productsList;
        private Products _productsInstance;
        private Products products;

        [SetUp]
        public async Task SetUp()
        {
            _productsList = new List<Product>
            {
                new Product { Id = 4, Name = "Keyboard Corsair", Categoria = new Categoria(8, "Tecnología") },
                new Product { Id = 8, Name = "Celular", Categoria = new Categoria(8, "Tecnología") },
                new Product { Id = 9, Name = "Celular", Categoria = new Categoria(8, "Tecnología") },
                new Product { Id = 10, Name = "Celular", Categoria = new Categoria(8, "Tecnología") },
                new Product { Id = 11, Name = "Celular", Categoria = new Categoria(8, "Tecnología") },
                new Product { Id = 12, Name = "Logitech Superlight", Categoria = new Categoria(8, "Tecnología") }
            };

            _productsInstance = Products.InitializeFromMemory(_productsList);
            var dbTest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(dbTest);
            DatabaseStore.Store_MySql();
            dbTest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(dbTest);
            products = await Products.InitializeAsync();
        }

        [Test]
        public async Task GetProductsByCategory_ReturnsProductsForValidCategoryId()
        {
            var categoryId = 8;
            var categoryIds = new[] { categoryId };
            var products = await _productsInstance.GetProductsByCategory(categoryIds);
            Assert.IsNotNull(products);
            Assert.IsInstanceOf<List<Product>>(products);
            Assert.AreEqual(8, products.Count); // Debe devolver 6 productos
        }

        [Test]
        public void GetProductsByCategory_ThrowsException_ForInvalidCategoryId()
        {
            int invalidCategoryId = -1;
            var invalidCategoryIds = new[] { invalidCategoryId };
            Assert.ThrowsAsync<ArgumentException>(async () => await _productsInstance.GetProductsByCategory(invalidCategoryIds));
        }

        [Test]
        public async Task SearchProducts_ReturnsPartialMatches()
        {
            var productName = "Celu";
            var categoryIds = new[] { 8 };
            var result = await _productsInstance.SearchProducts(productName, categoryIds);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public async Task SearchProducts_ReturnsExactMatches()
        {
            var productName = "Logitech Superlight";
            var categoryIds = new[] { 8 };
            var result = await _productsInstance.SearchProducts(productName, categoryIds);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Logitech Superlight", result[0].Name);
        }

        [Test]
        public async Task SearchProducts_NoMatches()
        {
            var productName = "Nonexistent Product";
            var categoryIds = new[] { 8 };
            var result = await _productsInstance.SearchProducts(productName, categoryIds);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
