using NUnit.Framework;
using Microsoft.Extensions.Caching.Memory;
using storeapi.Business;
using storeapi.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using core;
using storeapi.Database;

namespace UT
{
    public class InMemoryStoreDB : IStoreDB
    {
        private List<Product> _products = new List<Product>();

        public List<Product> InsertProduct(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.id == product.id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
            }
            else
            {
                _products.Add(product);
            }
            return _products;
        }
    }

    [TestFixture]
    public class InsertProductsLogicTests : IDisposable
    {
        private IMemoryCache _memoryCache;
        private InsertProductsLogic _insertProductsLogic;
        private InMemoryStoreDB _inMemoryStoreDB;

        [SetUp]
        public void Setup()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _inMemoryStoreDB = new InMemoryStoreDB();
            _insertProductsLogic = new InsertProductsLogic(_memoryCache, _inMemoryStoreDB);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCache.Dispose();
        }

        [Test]
        public void InsertProduct_NewProduct_AddsToCache()
        {
            // Arrange
            var newProduct = new Product
            {
                id = 1,
                Name = "New Product",
                Price = 99,
                ImageUrl = "http://example.com/image.png",
                Description = "New Product Description",
                Category = new Category { _id = 1, _name = "New Category" }
            };

            // Act
            var result = _insertProductsLogic.InsertProduct(newProduct);

            // Assert
            Assert.IsTrue(result.Any(p => p.id == newProduct.id && p.Name == newProduct.Name));
            Assert.IsTrue(_memoryCache.TryGetValue("Products", out List<Product> cachedProducts));
            Assert.IsTrue(cachedProducts.Any(p => p.id == newProduct.id && p.Name == newProduct.Name));
        }

        [Test]
        public void InsertProduct_ExistingProduct_UpdatesInCache()
        {
            // Arrange
            var existingProduct = new Product
            {
                id = 1,
                Name = "Updated Product",
                Price = 29,
                ImageUrl = "http://example.com/image.png",
                Description = "Updated Product Description",
                Category = new Category { _id = 1, _name = "Updated Category" }
            };
            _memoryCache.Set("Products", new List<Product> { existingProduct });

            var updatedProduct = new Product
            {
                id = 1,
                Name = "Updated Product",
                Price = 29,
                ImageUrl = "http://example.com/image.png",
                Description = "Updated Product Description",
                Category = new Category { _id = 1, _name = "Updated Category" }
            };

            // Act
            var result = _insertProductsLogic.InsertProduct(updatedProduct);

            // Assert
            var productInCache = result.FirstOrDefault(p => p.id == updatedProduct.id);
            Assert.NotNull(productInCache);
            Assert.AreEqual(updatedProduct.Name, productInCache.Name);
            Assert.AreEqual(updatedProduct.Price, productInCache.Price);
            Assert.AreEqual(updatedProduct.ImageUrl, productInCache.ImageUrl);
            Assert.AreEqual(updatedProduct.Description, productInCache.Description);
            Assert.AreEqual(updatedProduct.Category._name, productInCache.Category._name);
        }

        public void Dispose()
        {
            _memoryCache?.Dispose();
        }
    }
}
