using NUnit.Framework;
using Store_API.Models;
using Core.Models;

namespace Store_API.Tests
{
    [TestFixture]
    public class StoreTest
    {
        private Store store;

        [SetUp]
        public void Setup()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id= 1,
                    Name = "Iphone",
                    Description= "Producto nuevo",
                    Categoria  = new Category(1, "Electrónica")
                },
                new Product
                {
                    Id= 2,
                    Name = "Audifono",
                    Description= "Producto nuevo",
                    Categoria  = new Category(1, "Electrónica")
                },
                new Product
                {
                    Id= 3,
                    Name = "Mouse",
                    Description= "Producto nuevo",
                    Categoria  = new Category(2, "Hogar y oficina")
                },
            };

        }
/*
        [Test]
        public async Task TestGetProductByNameAndCategoryIdAsync_ExistingProduct()
        {
            // Arrange
            string productName = "Iphone";
            int categoryId = 1;

            // Act
            var product = await store.GetProductByNameAndCategoryIdAsync(productName, categoryId);

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(categoryId, product.Categoria.IdCategory);
        }

        [Test]
        public async Task TestGetProductByNameAndCategoryIdAsync_NonExistingProduct()
        {
            // Arrange
            string productName = "NonExistingProduct";
            int categoryId = 1;

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await store.GetProductByNameAndCategoryIdAsync(productName, categoryId));
        }
        */
    }
}