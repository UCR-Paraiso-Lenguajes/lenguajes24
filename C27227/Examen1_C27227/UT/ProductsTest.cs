using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using KEStoreApi;
using KEStoreApi.Data;

namespace Tests
{
    public class ProductsTest
    {
        private List<Product> _productsList;
        private Products _productsInstance;

        [SetUp]
        public async Task SetUp()
        {
            _productsList = new List<Product>
            {
                new Product { Id = 4, Name = "Keyboard Corsair", CategoriaId = 8, IsDeleted = false },
                new Product { Id = 8, Name = "Celular", CategoriaId = 8, IsDeleted = false },
                new Product { Id = 9, Name = "Celular", CategoriaId = 8, IsDeleted = false },
                new Product { Id = 10, Name = "Celular", CategoriaId = 8, IsDeleted = false },
                new Product { Id = 11, Name = "Celular", CategoriaId = 8, IsDeleted = true },
                new Product { Id = 12, Name = "Logitech Superlight", CategoriaId = 8, IsDeleted = false }
            };

            _productsInstance = Products.InitializeFromMemory(_productsList);
            var dbTest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(dbTest);
            DatabaseStore.Store_MySql();
            dbTest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(dbTest);
        }

        [Test]
        public async Task GetProductsByCategory_ReturnsProductsForValidCategoryId()
        {
            var categoryId = 8;
            var categoryIds = new[] { categoryId };
            var products = await _productsInstance.GetProductsByCategory(categoryIds);
            Assert.IsNotNull(products);
            Assert.AreEqual(6, products.Count()); // Excluding the deleted product
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
            Assert.AreEqual(4, result.Count());
        }

        [Test]
        public async Task SearchProducts_ReturnsExactMatches()
        {
            var productName = "Logitech Superlight";
            var categoryIds = new[] { 8 };
            var result = await _productsInstance.SearchProducts(productName, categoryIds);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Logitech Superlight", result.First().Name);
        }

        [Test]
        public async Task SearchProducts_NoMatches()
        {
            var productName = "Nonexistent Product";
            var categoryIds = new[] { 8 };
            var result = await _productsInstance.SearchProducts(productName, categoryIds);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
