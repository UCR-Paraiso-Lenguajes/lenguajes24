using Core;
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using NUnit.Framework;

namespace UT{
    
    [TestFixture]
    public class ProductManagementTesting{

        private List<Product> productsInserted ;

        [SetUp]
        public void SetUp(){
            productsInserted = new List<Product>();
        }

        //Test para los datos de consultas de ventas
        [Test]
        public async Task delegateTesting(){
            // Arrange
            ProductManagementLogic productManagementLogic = new ProductManagementLogic();
            productManagementLogic.onProductInserted = TestProductInsertedDelegate;

            // Act
            Product productToAdd = new Product { id = 1, name = "Test Product", /* Otras propiedades */ };
            await productManagementLogic.insertProductAsyncUT(productToAdd);

            // Assert
            Assert.IsTrue(productsInserted.Contains(productToAdd));            
        }

         private void TestProductInsertedDelegate(Product newProduct){
            productsInserted.Add(newProduct);
        }
    }
}