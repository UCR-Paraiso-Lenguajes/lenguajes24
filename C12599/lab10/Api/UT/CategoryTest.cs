using NUnit.Framework;
using storeapi.Models;
using System;
using core;

namespace storeapi.UT
{
    [TestFixture]
    public class CategoryTest
    {
        [SetUp]
        public void Setup()
        {
            // Configuración inicial
            var dbtestDefault = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";
            DataConnection.Init(dbtestDefault);
        }

        [Test]
        public void TestGetCategoryIdByName_ValidCategory_ReturnsCategoryId()
        {
            // Arrange
            Categories categories = new Categories();
            string categoryName = "Tecnología";
            string expectedId = "8";

            // Act
            string categoryId = categories.GetCategoryIdByName(categoryName);

            // Assert
            Assert.AreEqual(expectedId, categoryId, $"El ID de la categoría '{categoryName}' no coincide con el valor esperado.");
        }

        [Test]
        public void TestGetCategoryIdByName_InvalidCategory_ThrowsArgumentException()
        {
            // Arrange
            Categories categories = new Categories();
            string categoryName = "Ropa"; // Categoría inexistente

            // Act y Assert
            Assert.Throws<ArgumentException>(() => categories.GetCategoryIdByName(categoryName), $"Se esperaba una excepción ArgumentException para la categoría '{categoryName}'.");
        }

        [Test]
        public void TestGetCategoryIdByName_NullCategory_ThrowsArgumentNullException()
        {
            // Arrange
            Categories categories = new Categories();

            // Act y Assert
            Assert.Throws<ArgumentNullException>(() => categories.GetCategoryIdByName(null), "Se esperaba una excepción ArgumentNullException para un nombre de categoría nulo.");
        }
    }
}
