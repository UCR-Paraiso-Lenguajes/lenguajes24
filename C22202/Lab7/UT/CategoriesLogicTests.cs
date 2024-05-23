using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShopApi.Models;

namespace ShopApi.Tests
{
    [TestFixture]
    public class CategoriesLogicTests
    {
        private List<Category> testCategories;
        private CategoriesLogic categoriesLogic;

        [SetUp]
        public void SetUp()
        {
            testCategories = new List<Category>
            {
                new Category(1, "Mouse"),
                new Category(2, "Monitores"),
                new Category(3, "SSD"),
                new Category(4, "Discos Duros"),
                new Category(5, "CPU"),
                new Category(6, "Tarjetas Gráficas"),
                new Category(7, "Teclados"),
                new Category(8, "Memorias RAM"),
                new Category(9, "Headsets"),
                new Category(10, "Fuentes de poder"),
                new Category(11, "Cases"),
                new Category(12, "Tarjetas Madre"),
                new Category(13, "Micrófonos")
            };

            testCategories.Sort((category1, category2) => string.Compare(category1.name, category2.name));

            // Usar reflexión para invocar el constructor privado
            var constructor = typeof(CategoriesLogic).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(List<Category>) }, null);

            categoriesLogic = (CategoriesLogic)constructor.Invoke(new object[] { testCategories });
            CategoriesLogic.Instance = categoriesLogic;
        }

        [Test]
        public void GetCategories_ReturnsAllCategories()
        {
            // Act
            var results = categoriesLogic.GetCategories().ToList();

            // Assert
            Assert.AreEqual(testCategories.Count, results.Count);
            foreach (var category in testCategories)
            {
                Assert.Contains(category, results);
            }
        }

        [Test]
        public void GetCategories_WhenNoCategoriesAvailable_ThrowsException()
        {
            // Arrange
            var emptyCategoriesLogic = CreateEmptyCategoriesLogic();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => emptyCategoriesLogic.GetCategories());
            Assert.That(ex.Message, Is.EqualTo("No categories available."));
        }

        // Método privado para crear una instancia de CategoriesLogic con una lista vacía de categorías
        private CategoriesLogic CreateEmptyCategoriesLogic()
        {
            // Usar reflexión para invocar el constructor privado con una lista vacía
            var constructor = typeof(CategoriesLogic).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(List<Category>) }, null);

            return (CategoriesLogic)constructor.Invoke(new object[] { new List<Category>() });
        }
    }
}
