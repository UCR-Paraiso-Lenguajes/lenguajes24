using NUnit.Framework;
using storeapi.Models;
using System.Collections.Generic;
using storeapi.Database;
using core;
namespace UT
{
    [TestFixture]
    public class ProductsTests
    {
        private Products productsManager;

        [SetUp]
        public void Setup()
        {
            // Configurar la conexión a la base de datos para las pruebas
            var dbTestConnectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";
            DataConnection.Init(dbTestConnectionString);

            // Inicializar el administrador de productos antes de cada prueba
            productsManager = new Products();
        }

        [Test]
        public void LoadProductsFromDatabase_ValidCategoryID_ReturnsMatchingProducts()
        {
            // Arrange
            int categoryId = 1; // Simulamos una categoría válida

            // Act
            IEnumerable<Product> loadedProducts = productsManager.LoadProductsFromDatabase(categoryId);

            // Assert
            Assert.IsNotNull(loadedProducts, "La lista de productos cargada no debe ser nula.");
            Assert.AreEqual(2, ((List<Product>)loadedProducts).Count, "Se esperan dos productos con el ID de categoría '1'.");

            // Verificar los detalles de los productos cargados
            foreach (var product in loadedProducts)
            {
                Assert.AreEqual(categoryId, product.Category.Id, "El ID de categoría del producto debe coincidir con el valor esperado.");
            }
        }

        [Test]
        public void LoadProductsFromDatabase_InvalidCategoryID_ReturnsNoProducts()
        {
            // Arrange
            int invalidCategoryId = -1; // Simulamos una categoría no válida

            // Act
            IEnumerable<Product> loadedProducts = productsManager.LoadProductsFromDatabase(invalidCategoryId);

           
            Assert.IsNotNull(loadedProducts, "La lista de productos cargada no debe ser nula.");
            Assert.IsEmpty((List<Product>)loadedProducts, "No se esperan productos para un ID de categoría no válido.");
        }

     
    }
}
