using Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class CategoriesTest
    {
        [Test]
        [TestCase(1, "Electrónica")]
        [TestCase(2, "Hogar y oficina")]
        [TestCase(3, "Entretenimiento")]
        [TestCase(4, "Tecnología")]
        public void TestGetCategoryById(int categoryId, string expectedName)
        {
            var expectedCategory = new Category(categoryId, expectedName);
            bool categoryFound = false;

            Category resultCategory;
            try
            {
                resultCategory = Categories.GetCategoryById(categoryId);
                categoryFound = true;
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception: {ex.Message}");
                resultCategory = default;
            }

            Assert.IsTrue(categoryFound, "Category should have been found");
            Assert.AreEqual(expectedCategory.IdCategory, resultCategory.IdCategory);
            Assert.AreEqual(expectedCategory.NameCategory, resultCategory.NameCategory);
        }

        [Test]
        public void TestGetCategories()
        {
            var expectedCategories = new Category[]
            {
                new Category(1, "Electrónica"),
                new Category(2, "Hogar y oficina"),
                new Category(3, "Entretenimiento"),
                new Category(4, "Tecnología"),
            };

            IEnumerable<Category> resultCategories = null;
            try
            {
                resultCategories = Categories.GetCategories();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception: {ex.Message}");
            }

            Assert.NotNull(resultCategories, "Categories should not be null");
            Assert.AreEqual(expectedCategories.Length, resultCategories.Count());
            foreach (var expectedCategory in expectedCategories)
            {
                Assert.IsTrue(resultCategories.Any(c => c.IdCategory == expectedCategory.IdCategory && c.NameCategory == expectedCategory.NameCategory));
            }
        }
    }
}