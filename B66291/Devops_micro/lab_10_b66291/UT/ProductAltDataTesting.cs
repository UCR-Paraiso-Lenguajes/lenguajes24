using NUnit.Framework;
using core;
using core.DataBase;
using core.Models;
using core.Business;
using Moq;
using System;
using System.Threading.Tasks;

namespace UT
{
    public class ProductAltDataTesting
    {
        private ProductAltData _productData;

        [SetUp]
        public void Setup()
        {
            var myDbtest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            StoreDb.CrearDatosSync();

            _productData = new ProductAltData();
        }

        [Test]
        public async Task InsertarProductoAsync_CasoExitoso()
        {
            var product = new Product
            {
                name = "Test Product",
                description = "Test Description",
                price = 10.99m,
                imageUrl = "test.jpg",
                pcant = 5,
                category = new Categories.Category { id = 1 }
            };

            var mockDelegate = new Mock<ProductAltLogic.nuevoProuductoD>();
            await _productData.InsertarProductoAsync(product, mockDelegate.Object);
            mockDelegate.Verify(d => d(product), Times.Once);
        }


        [Test]
        public async Task InsertarProductoAsync_ProductoVacio()
        {

            Product product = null;
            var mockDelegate = new Mock<ProductAltLogic.nuevoProuductoD>();
            var productAltData = new ProductAltData();
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await productAltData.InsertarProductoAsync(product, mockDelegate.Object);
            });
            Assert.AreEqual("El producto a agregar se encuentra vacio", ex.Message);
        }

        [Test]
        public async Task InsertarProductoAsync_DelegateVacio()
        {
            // Arrange
            var product = new Product
            {
                name = "Test Product",
                description = "Test Description",
                price = 100,
                imageUrl = "https://example.com/product.jpg",
                pcant = 10,
                category = new Categories.Category { id = 1 }
            };

            ProductAltLogic.nuevoProuductoD nuevoProductoDelegate = null;
            var productAltData = new ProductAltData();

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await productAltData.InsertarProductoAsync(product, nuevoProductoDelegate);
            });

            Assert.AreEqual("La funcion delegate no se ha enviado por parametro", ex.ParamName);
        }
    }
}