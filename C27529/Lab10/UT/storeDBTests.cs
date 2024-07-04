using NUnit.Framework;
using storeApi;
using storeApi.db;
using storeApi.Models;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace storeApi.Tests
{
    public class StoreDBTests
    {
        private StoreDB storeDB;

        [SetUp]
        public async Task Setup()
        {
            // Asegúrate de que esta cadena de conexión sea válida para tu entorno de pruebas
            ConnectionDB.Init("Server=localhost;Database=store;Uid=root;Pwd=123456;");
            storeDB = new StoreDB();

            // Crear la base de datos y las tablas antes de cada prueba
            await StoreDB.CreateMysql();
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnAllProducts()
        {
            // Act
            var products = await StoreDB.GetProductsAsync();

            // Assert
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
        }

        [Test]
        public async Task CreateMysql_ShouldCreateDatabaseAndTables()
        {
            // Act
            await StoreDB.CreateMysql();

            // Assert
            var products = await StoreDB.GetProductsAsync();
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
        }

        [Test]
        public async Task AddMessageAsync_ValidContent_ShouldAddMessage()
        {
            // Arrange
            string content = "This is a test message";

            // Act
            int messageId = await StoreDB.AddMessageAsync(content);

            // Assert
            Assert.Greater(messageId, 0);

            var messages = await StoreDB.GetMessagesByContentAsync(content);
            Assert.IsNotEmpty(messages);
            Assert.AreEqual(content, messages[0]["content"]);
        }

        [Test]
        public void AddMessageAsync_InvalidContent_ShouldThrowArgumentException()
        {
            // Arrange
            string content = "";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await StoreDB.AddMessageAsync(content));
            Assert.That(ex.Message, Is.EqualTo("Content cannot be null or empty (Parameter 'content')"));
        }

        


        [Test]
        public async Task GetMessagesByContentAsync_ValidContent_ShouldReturnMessages()
        {
            // Arrange
            string content = "Unique test message";
            await StoreDB.AddMessageAsync(content);

            // Act
            var messages = await StoreDB.GetMessagesByContentAsync(content);

            // Assert
            Assert.IsNotEmpty(messages);
            Assert.AreEqual(content, messages[0]["content"]);
        }

        [Test]
        public void GetMessagesByContentAsync_InvalidContent_ShouldThrowArgumentException()
        {
            // Arrange
            string content = "";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await StoreDB.GetMessagesByContentAsync(content));
            Assert.That(ex.Message, Is.EqualTo("Content cannot be null or empty (Parameter 'content')"));
        }


        [Test]
        public async Task AddMessageAsync_DuplicateContent_ShouldReturnExistingMessageId()
        {
            // Arrange
            string content = "Duplicate test message";

            // Act
            int firstMessageId = await StoreDB.AddMessageAsync(content);
            int secondMessageId = await StoreDB.AddMessageAsync(content);

            // Assert
            Assert.AreEqual(firstMessageId, secondMessageId);

            var messages = await StoreDB.GetMessagesByContentAsync(content);
            Assert.AreEqual(1, messages.Count); // Ensure the message is not duplicated
            Assert.AreEqual(content, messages[0]["content"]);
        }


        [Test]
        public async Task GetProductsAsync_AfterCreateMysql_ShouldReturnAllProducts()
        {
            // Arrange
            await StoreDB.CreateMysql();

            // Act
            var products = await StoreDB.GetProductsAsync();

            // Assert
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
            Assert.AreEqual(32, products.Count); // Assuming the initial products count is 32 as per CreateMysql method
        }


       

    }
}
