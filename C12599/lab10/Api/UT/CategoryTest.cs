using NUnit.Framework;
using System;
using System.Linq;
using storeapi.Models;

namespace storeapi.Tests
{
    public class CategoriesTests
    {
        [Test]
        public void Categories_AreSortedAlphabetically()
        {
            // Arrange
            var categories = new Categories();

            // Act
            var sortedCategories = categories.ListCategories.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList();

            // Assert
            CollectionAssert.AreEqual(categories.ListCategories, sortedCategories);
        }

        [Test]
        public void Categories_ContainExpectedCategories()
        {
            // Arrange
            var expectedCategories = new[]
            {
                new Category(1, "Electrónica"),
                new Category(2, "Moda"),
                new Category(3, "Hogar y jardín"),
                new Category(4, "Deportes y actividades al aire libre"),
                new Category(5, "Belleza y cuidado personal"),
                new Category(6, "Alimentación y bebidas"),
                new Category(7, "Libros y entretenimiento"),
                new Category(8, "Tecnología"),
                new Category(9, "Deportes")
            };

            var categories = new Categories();

            // Act
            var actualCategories = categories.ListCategories.ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }
    }
}
