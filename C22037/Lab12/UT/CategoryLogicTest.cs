using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using TodoApi;
using TodoApi.Business;
using TodoApi.Models;

namespace UT
{
    public class CategoryLogicTest
    {
        private CategoryLogic categoryLogic;
        private Categories categories;

        [SetUp]
        public void Setup()
        {
            categories = new Categories();

            var initialCategories = new Categories.CategorySt[]
            {
                new Categories.CategorySt(1, "Food and Beverages"),
                new Categories.CategorySt(2, "Beauty and Personal Care")
            };

            var products = new List<Product>
            {
                new Product("Apple", "image/apple.jpg", 1.99m, "Fresh apple", 1, categories.GetType(1)),
                new Product("Banana", "image/banana.jpg", 0.99m, "Ripe banana", 2, categories.GetType(1)),
                new Product("Shampoo", "image/shampoo.jpg", 5.99m, "For shiny hair", 3, categories.GetType(2))
            };

            categoryLogic = new CategoryLogic(initialCategories, products);
        }

        [Test]
        public async Task GetCategoryByIdAsync_ExistingId_ReturnsProducts()
        {
            int existingCategoryId = 1;

            var products = await categoryLogic.GetCategoriesByIdAsync(new List<int> { existingCategoryId });

            Assert.IsNotNull(products);
            Assert.IsInstanceOf<IEnumerable<Product>>(products);
            Assert.GreaterOrEqual(((List<Product>)products).Count, 1);
        }

        [Test]
        public async Task GetCategoryByIdAsync_NonExistingId_ReturnsEmptyList()
        {
            int nonExistingCategoryId = 7;

            var products = await categoryLogic.GetCategoriesByIdAsync(new List<int> { nonExistingCategoryId });

            Assert.IsNotNull(products);
            Assert.IsEmpty(products);
        }

        [Test]
        public async Task GetCategoryByIdAsync_MultipleExistingIds_ReturnsAllProducts()
        {
            var existingCategoryIds = new List<int> { 1, 2 };

            var products = await categoryLogic.GetCategoriesByIdAsync(existingCategoryIds);

            Assert.IsNotNull(products);
            Assert.IsInstanceOf<IEnumerable<Product>>(products);
            Assert.AreEqual(3, ((List<Product>)products).Count);
        }

        [Test]
        public async Task GetProductsBySearchAsync_NullSearchTerm_ThrowsArgumentNullException()
        {
            string searchTerm = null;

            Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryLogic.GetProductsBySearchAsync(searchTerm, null));
        }
    }
}