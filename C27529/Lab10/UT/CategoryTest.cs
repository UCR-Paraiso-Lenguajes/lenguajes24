using NUnit.Framework;
using storeApi;

namespace UT
{
    public class CategoryTest
    {
        private Category _category;

        [SetUp]
        public void Setup()
        {
            _category = new Category();
        }

        [Test]
        public void GetCategories_ShouldReturnSortedCategories()
        {
            var categories = _category.GetCategories();

            var previousCategoryName = string.Empty;
            foreach (var category in categories)
            {
                Assert.IsTrue(string.Compare(category.Name, previousCategoryName) >= 0);
                previousCategoryName = category.Name;
            }
        }

        [Test]
        public void GetCategoryNames_ShouldReturnSortedCategoryNames()
        {
            var categoryNames = _category.GetCategoryNames();

            var previousCategoryName = string.Empty;
            foreach (var categoryName in categoryNames)
            {
                Assert.IsTrue(string.Compare(categoryName.Name, previousCategoryName) >= 0);
                previousCategoryName = categoryName.Name;
            }
        }

        [Test]
        public void GetCategoryById_ShouldReturnCorrectCategory()
        {
            var categoryId = 3;
            var category = _category.GetCategoryById(categoryId);

            Assert.IsNotNull(category);
            Assert.AreEqual(categoryId, category.Id);
            Assert.AreEqual("Consolas", category.Name);
        }
    }
}
