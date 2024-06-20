using NUnit.Framework;
using TodoApi.Models;
using TodoApi.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi;

namespace UT
{
    public class ProductAddTest
    {
        private Store store;
        private StoreDB storeDB;
        private Categories category;

        [SetUp]
        public void Setup()
        {
            storeDB = new StoreDB();
            category = new Categories();

            Storage.Init("Server=localhost;Port=3407;Database=mysql;Uid=root;Pwd=123456;");
            StoreDB.CreateMysql();
            Storage.Init("Server=localhost;Port=3407;Database=store;Uid=root;Pwd=123456;");

            var products = new List<Product>
            {
                new Product("Producto 1", "https://example.com/img1.jpg", 10, "Descripción 1", 1, category.GetType(1)),
                new Product("Producto 2", "https://example.com/img2.jpg", 20, "Descripción 2", 2, category.GetType(2))
            };

            store = new Store(products, 13, storeDB);
        }

        [Test]
        public async Task AddProductToList()
        {
            var productAdd = new ProductAdd("Nuevo Producto", "https://example.com/img3.jpg", 30, "Nueva Descripción", 1);

            await store.AddProduct(productAdd, 3);

            Assert.AreEqual(3, store.Products.Count);
            var addedProduct = store.Products.Find(p => p.Id == 3);
            Assert.IsNotNull(addedProduct);
            Assert.AreEqual("Nuevo Producto", addedProduct.Name);
            Assert.AreEqual(30, addedProduct.Price);
        }

        [Test]
        public async Task InsertProductAsyncAndCallDelegate()
        {
            var productAdd = new ProductAdd("Test Product", "https://example.com/test.jpg", 99.99M, "Test Description", 1);

            await storeDB.InsertProductAsync(productAdd);

            var products = await StoreDB.GetProductsAsync();
            var insertedProduct = products.LastOrDefault();

            Assert.IsNotNull(insertedProduct);
            Assert.AreEqual("Test Product", insertedProduct.Name);
            Assert.AreEqual(99.99M, insertedProduct.Price);
            Assert.AreEqual("https://example.com/test.jpg", insertedProduct.ImageURL);
            Assert.AreEqual("Test Description", insertedProduct.Description);
            Assert.AreEqual(1, insertedProduct.Category.Id);
        }
    }
}
