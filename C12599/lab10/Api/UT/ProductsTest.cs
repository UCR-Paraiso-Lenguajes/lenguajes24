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
         
        [SetUp]
              public void Setup()
    {
        
        var dbtestDefault = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";
        DataConnection.Init(dbtestDefault);
    }
        [Test]
        public void LoadProductsFromDatabase_ValidCategoryID_ReturnsMatchingProducts()
        {
            // Arrange
            int categoryId = 1; // Simulamos una categoría válida
            Products productsManager = new Products();

            // Creamos datos de productos simulados para cargar
            List<string[]> productData = new List<string[]>
            {
                new string[] { "1", "Product 1", "10.99", "https://example.com/product1.jpg", "Description of Product 1", "1" },
                new string[] { "2", "Product 2", "20.99", "https://example.com/product2.jpg", "Description of Product 2", "2" },
                new string[] { "3", "Product 3", "30.99", "https://example.com/product3.jpg", "Description of Product 3", "1" }
            };

            // Simulamos el comportamiento del método RetrieveDatabaseInfo de StoreDB
            StoreDB.CreateMysql();

            // Act
            IEnumerable<Product> loadedProducts = productsManager.LoadProductsFromDatabase(categoryId);

            // Assert
            Assert.IsNotNull(loadedProducts, "La lista de productos cargada no debe ser nula.");
            Assert.AreEqual(1, ((List<Product>)loadedProducts).Count, "Se esperan dos productos con el ID de categoría '1'.");

            // Verificamos los detalles de los productos cargados
            foreach (var product in loadedProducts)
            {
                Assert.AreEqual(1, product.CategoryID, "El ID de categoría del producto debe coincidir con el valor esperado.");
            }
        }

       
        public void LoadProductsFromDatabase_InvalidCategoryID_ThrowsArgumentException(int categoryId)
        {
            // Arrange
            Products productsManager = new Products();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productsManager.LoadProductsFromDatabase(categoryId), "Se esperaba una excepción ArgumentException para un categoryId nulo o vacío.");
        }

 }
  }
    
      

