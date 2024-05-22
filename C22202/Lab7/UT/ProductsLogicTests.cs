using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShopApi.Models;

namespace ShopApi.Tests
{
    [TestFixture]
    public class ProductsLogicTests
    {
        private List<Product> testProducts;
        private Dictionary<int, List<Product>> testProductsDictionary;
        private ProductsLogic productsLogic;

        [SetUp]
        public void SetUp()
        {
            testProducts = new List<Product>
            {
                new Product { id = 1, name = "Apple", category = 1, price = 1.00M, imgSource = "apple.jpg" },
                new Product { id = 2, name = "Banana", category = 1, price = 0.50M, imgSource = "banana.jpg" },
                new Product { id = 3, name = "Carrot", category = 2, price = 0.30M, imgSource = "carrot.jpg" },
                new Product { id = 4, name = "Date", category = 3, price = 1.20M, imgSource = "date.jpg" },
                new Product { id = 5, name = "Eggplant", category = 2, price = 0.80M, imgSource = "eggplant.jpg" }
            };

            testProductsDictionary = new Dictionary<int, List<Product>>
            {
                { 1, new List<Product> { testProducts[0], testProducts[1] } },
                { 2, new List<Product> { testProducts[2], testProducts[4] } },
                { 3, new List<Product> { testProducts[3] } }
            };

            // Usar reflexión para invocar el constructor privado
            var constructor = typeof(ProductsLogic).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(IEnumerable<Product>), typeof(Dictionary<int, List<Product>>) }, null);

            productsLogic = (ProductsLogic)constructor.Invoke(new object[] { testProducts, testProductsDictionary });
            ProductsLogic.Instance = productsLogic;
        }

        [Test]
        public void SearchProducts_ReturnsCorrectResults()
        {
            // Arrange
            var searchQuery = "a";

            // Act
            var results = productsLogic.searchProducts(testProducts, searchQuery).ToList();

            // Assert
            Assert.AreEqual(4, results.Count);
            Assert.Contains(testProducts[0], results);
            Assert.Contains(testProducts[1], results);
            Assert.Contains(testProducts[2], results);
            Assert.Contains(testProducts[3], results);
        }

        [Test]
        public void GetProductsCategory_WithValidCategoryIds_ReturnsCorrectProducts()
        {
            // Arrange
            var categoryIds = new List<int> { 1, 2 };

            // Act
            var results = productsLogic.GetProductsCategory(categoryIds).ToList();

            // Assert
            Assert.AreEqual(4, results.Count);
            Assert.Contains(testProducts[0], results);
            Assert.Contains(testProducts[1], results);
            Assert.Contains(testProducts[2], results);
            Assert.Contains(testProducts[4], results);
        }

        [Test]
        public void GetProductsCategory_WithInvalidCategoryIds_ReturnsEmptyList()
        {
            // Arrange
            var categoryIds = new List<int> { 99 };

            // Act
            var results = productsLogic.GetProductsCategory(categoryIds).ToList();

            // Assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void GetProductsCategory_WithZeroCategoryId_ReturnsAllProducts()
        {
            // Arrange
            var categoryIds = new List<int> { 0 };

            // Act
            var results = productsLogic.GetProductsCategory(categoryIds).ToList();

            // Assert
            Assert.AreEqual(5, results.Count);
            Assert.Contains(testProducts[0], results);
            Assert.Contains(testProducts[1], results);
            Assert.Contains(testProducts[2], results);
            Assert.Contains(testProducts[3], results);
            Assert.Contains(testProducts[4], results);
        }

        [Test]
        public void GetProductsCategory_WithEmptyCategoryIds_ThrowsArgumentException()
        {
            // Arrange
            var categoryIds = new List<int>();

            // Act & Assert
            Assert.Throws<System.ArgumentException>(() => productsLogic.GetProductsCategory(categoryIds));
        }
    }
}
