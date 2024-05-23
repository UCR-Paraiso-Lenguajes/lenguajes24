using Core.Models;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CategoriesTest
    {
        [Test]
        public void TestGetCategoryById()
        {
            var expectedCategory = new ProductCategoryStruct(1, "Electrónica");
            bool categoryFound = false;

            ProductCategoryStruct resultCategory;
            try
            {
                resultCategory = Category.GetCategoryById(1);
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
            var category = Category.Instance;
            var expectedCategories = new ProductCategoryStruct[]
            {
                new ProductCategoryStruct(1, "Electrónica"),
                new ProductCategoryStruct(2, "Moda"),
                new ProductCategoryStruct(3, "Hogar y jardín"),
                new ProductCategoryStruct(4, "Deportes y actividades al aire libre"),
                new ProductCategoryStruct(5, "Belleza y cuidado personal"),
                new ProductCategoryStruct(6, "Alimentación y bebidas"),
                new ProductCategoryStruct(7, "Libros y entretenimiento"),
                new ProductCategoryStruct(8, "Tecnología"),
                new ProductCategoryStruct(9, "Deportes")
            };

            IEnumerable<ProductCategoryStruct> resultCategories = null;
            try
            {
                resultCategories = Category.GetCategories();
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