using NUnit.Framework;
using storeapi.Models;
using System.Collections.Generic;
using storeapi.Database;

namespace UT
{
    [TestFixture]
    public class CategoriesTests
    {
        [Test]
        public void Categories_ContainExpectedCategories()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category(6, "Alimentación y bebidas"),
                new Category(5, "Belleza y cuidado personal"),
                new Category(9, "Deportes"),
                new Category(4, "Deportes y actividades al aire libre"),
                new Category(1, "Electrónica"),
               new Category(3, "Hogar y jardín"),
               new Category(7, "Libros y entretenimiento"),
                new Category(2, "Moda"),
                new Category(8, "Tecnología"),
                
            };

            var categories = new Categories();

            // Act
            var actualCategories = categories.ListCategories;

            // Assert
            Assert.AreEqual(expectedCategories.Count, actualCategories.Count, "La cantidad de categorías esperadas y obtenidas no coincide.");

            for (int i = 0; i < expectedCategories.Count; i++)
            {
                Assert.AreEqual(expectedCategories[i].Id, actualCategories[i].Id, $"El Id de la categoría {i + 1} no coincide.");
                Assert.AreEqual(expectedCategories[i].Name, actualCategories[i].Name, $"El Nombre de la categoría {i + 1} no coincide.");
            }
        }
    }
}

