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
        public void Setup()
        {
            ConnectionDB.Init("Server=172.20.192.1;Database=mysql;Uid=root;Pwd=123456;");
            storeDB = new StoreDB();
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
    }
}
