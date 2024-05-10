using NUnit.Framework;
using storeapi.Models;

using System.Collections.Generic;

namespace UT
{
    public class CategoriesTests
    {
        [Test]
        public void GetCategoryId_ValidCategoryId_ReturnsMatchingId()
        {
            // Arrange
            var categories = new Categories();
            var categoryId = "4"; // Category Id as string
            var expectedId = 4;

            // Act
            var result = categories.GetCategoryId(categoryId);

            // Assert
            Assert.AreEqual(expectedId, result);
        }

        [Test]
        public void GetCategoryId_InvalidCategoryId_ThrowsException()
        {
            // Arrange
            var categories = new Categories();
            var categoryId = "invalid";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => categories.GetCategoryId(categoryId));
        }

    
        [Test]
        public void GetCategoryId_NullOrEmptyCategoryId_ThrowsException()
        {
            // Arrange
            var categories = new Categories();
            string categoryId = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => categories.GetCategoryId(categoryId));
        }
    }
}

