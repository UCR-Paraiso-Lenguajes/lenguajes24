using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TodoApi;
using TodoApi.Models;

namespace UT
{
    public class StoreTests
    {
        private List<Product> sampleProducts;
        Categories categories = new Categories();

        [SetUp]
        public void Setup()
        {  
            sampleProducts = new List<Product>
            {
                new Product("Product1", "url1", 100, "Description1", 1, categories.GetType(1), 1),
                new Product("Product2", "url2", 200, "Description2", 2, categories.GetType(2), 1)
            };
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenProductsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Store(null, 13, null));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenProductsAreEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Store(new List<Product>(), 13, null));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenTaxPercentageIsInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Store(sampleProducts, -1, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Store(sampleProducts, 101, null));
        }

        [Test]
        public async Task RemoveProductAsync_ShouldThrowException_WhenProductIdIsInvalid()
        {
            var store = new Store(sampleProducts, 13, null);
            Assert.ThrowsAsync<ArgumentException>(async () => await store.RemoveProductAsync(0));
            Assert.ThrowsAsync<ArgumentException>(async () => await store.RemoveProductAsync(-1));
        }

        [Test]
        public async Task GetProductsByCategoryAsync_ShouldThrowException_WhenCategoryIdIsInvalid()
        {
            var store = new Store(sampleProducts, 13, null);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await store.GetProductsByCategoryAsync(0));
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await store.GetProductsByCategoryAsync(-1));
        }
    }
}