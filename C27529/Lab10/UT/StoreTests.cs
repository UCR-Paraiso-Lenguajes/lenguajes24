using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeApi.Tests
{
    public class StoreTests
    {
        private List<Product> mockProducts;
        private Store store;

        [SetUp]
        public void Setup()
        {
            mockProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 100, ImageURL = "url1", Category = new Category.ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 2, Name = "Product2", Description = "Description2", Price = 200, ImageURL = "url2", Category = new Category.ProductCategory { Id = 2, Name = "Category2" } },
                new Product { Id = 3, Name = "Product3", Description = "Description3", Price = 300, ImageURL = "url3", Category = new Category.ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 4, Name = "Product4", Description = "Description4", Price = 400, ImageURL = "url4", Category = new Category.ProductCategory { Id = 2, Name = "Category2" } }
            };

            store = new Store(mockProducts);
        }

        [Test]
        public async Task GetFilteredProductsAsync_ShouldReturnFilteredProducts()
        {
            // Act
            var filteredStore = await store.GetFilteredProductsAsync(new List<int> { 1 });
            var filteredProducts = filteredStore.Products.ToList();

            // Assert
            Assert.NotNull(filteredProducts);
            Assert.AreEqual(2, filteredProducts.Count); // Only products with Category Id 1 should be returned
            Assert.IsTrue(filteredProducts.All(p => p.Category.Id == 1));
        }

        [Test]
        public void GetFilteredTextProducts_ShouldReturnFilteredProductsByText()
        {
            // Act
            var filteredProducts = store.GetFilteredTextProducts("Product1");

            // Assert
            Assert.NotNull(filteredProducts);
            Assert.AreEqual(1, filteredProducts.Count);
            Assert.AreEqual("Product1", filteredProducts.First().Name);
        }


       
    }
}
