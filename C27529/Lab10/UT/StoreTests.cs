using NUnit.Framework;
using storeApi;
using storeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeApi.Tests
{
    public class StoreTests
    {
        private List<Product> testProducts;
        private Category.ProductCategory testCategory;

        [SetUp]
        public void Setup()
        {
            testCategory = new Category.ProductCategory { Id = 1, Name = "Electronics" };

            testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.0m, Description = "Description1", ImageURL = "http://image1.com", Category = testCategory },
                new Product { Id = 2, Name = "Product2", Price = 20.0m, Description = "Description2", ImageURL = "http://image2.com", Category = testCategory }
            };

            // Ensure the environment is set to Testing for unit tests
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        }

        [Test]
        public void StoreConstructor_ValidProducts_ShouldInitializeCorrectly()
        {
            // Act
            var store = new Store(testProducts, true); // Utiliza el constructor adicional

            // Assert
            Assert.AreEqual(testProducts, store.Products);
            Assert.NotNull(store.CategoriesProducts);
            Assert.NotNull(store.CategoriesNames);
        }

        [Test]
        public void StoreConstructor_NullProducts_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Store(null, true)); // Utiliza el constructor adicional
        }

        [Test]
        public async Task GetFilteredProductsAsync_ValidCategoryIds_ShouldReturnFilteredProducts()
        {
            // Arrange
            var store = new Store(testProducts, true); // Utiliza el constructor adicional
            var categoryIds = new List<int> { testProducts.First().Category.Id };

            // Act
            var filteredStore = await store.GetFilteredProductsAsync(categoryIds);

            // Assert
            Assert.AreEqual(2, filteredStore.Products.Count());
        }

        [Test]
        public void GetFilteredTextProducts_ValidSearchText_ShouldReturnFilteredProducts()
        {
            // Arrange
            var store = new Store(testProducts, true); // Utiliza el constructor adicional
            var searchText = "Product1";

            // Act
            var filteredProducts = store.GetFilteredTextProducts(searchText);

            // Assert
            Assert.AreEqual(1, filteredProducts.Count);
            Assert.AreEqual("Product1", filteredProducts.First().Name);
        }

        [Test]
        public void AddProductToStore_ValidProduct_ShouldAddProduct()
        {
            // Arrange
            var store = new Store(testProducts, true); // Utiliza el constructor adicional

            var newProduct = new Product
            {
                Id = 3,
                Name = "Product3",
                Price = 30.0m,
                Description = "Description3",
                ImageURL = "http://image3.com",
                Category = testCategory
            };

            // Act
            store.AddProductToStore(newProduct);

            // Assert
            Assert.Contains(newProduct, store.Products.ToList());
        }

        [Test]
        public void AddProductToStore_NullProduct_ShouldThrowArgumentNullException()
        {
            // Arrange
            var store = new Store(testProducts, true); // Utiliza el constructor adicional

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => store.AddProductToStore(null));
        }

        [Test]
        public void AddProductToStore_InvalidProduct_ShouldThrowArgumentNullException()
        {
            // Arrange
            var store = new Store(testProducts, true); // Utiliza el constructor adicional
            var invalidProduct = new Product
            {
                Id = 4,
                Name = null, // Invalid Name
                Price = 40.0m,
                Description = "Description4",
                ImageURL = "http://image4.com",
                Category = testCategory
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => store.AddProductToStore(invalidProduct));
        }
    }
}
