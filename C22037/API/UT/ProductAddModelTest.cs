using System;
using NUnit.Framework;
using TodoApi.Models;

namespace UT
{
    public class ProductAddModelTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ProductAdd_ValidParameters_CreatesInstance()
        {
            string name = "Apple";
            string imageUrl = "image/apple.jpg";
            decimal price = 1.99m;
            string description = "Fresh apple";
            int category = 1;

            var product = new ProductAdd(name, imageUrl, price, description, category);

            Assert.IsNotNull(product);
            Assert.AreEqual(name, product.name);
            Assert.AreEqual(imageUrl, product.imageUrl);
            Assert.AreEqual(price, product.price);
            Assert.AreEqual(description, product.description);
            Assert.AreEqual(category, product.category);
        }

        [Test]
        public void ProductAdd_NullOrEmptyName_ThrowsArgumentException()
        {
            string name = null;
            string imageUrl = "image/apple.jpg";
            decimal price = 1.99m;
            string description = "Fresh apple";
            int category = 1;

            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, price, description, category));
            Assert.Throws<ArgumentException>(() => new ProductAdd("", imageUrl, price, description, category));
        }

        [Test]
        public void ProductAdd_NullOrEmptyImageUrl_ThrowsArgumentException()
        {
            string name = "Apple";
            string imageUrl = null;
            decimal price = 1.99m;
            string description = "Fresh apple";
            int category = 1;

            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, price, description, category));
            Assert.Throws<ArgumentException>(() => new ProductAdd(name, "", price, description, category));
        }

        [Test]
        public void ProductAdd_NegativeOrZeroPrice_ThrowsArgumentException()
        {
            string name = "Apple";
            string imageUrl = "image/apple.jpg";
            string description = "Fresh apple";
            int category = 1;

            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, 0, description, category));
            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, -1.99m, description, category));
        }

        [Test]
        public void ProductAdd_NullOrEmptyDescription_ThrowsArgumentException()
        {
            string name = "Apple";
            string imageUrl = "image/apple.jpg";
            decimal price = 1.99m;
            string description = null;
            int category = 1;

            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, price, description, category));
            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, price, "", category));
        }

        [Test]
        public void ProductAdd_NegativeOrZeroCategory_ThrowsArgumentException()
        {
            string name = "Apple";
            string imageUrl = "image/apple.jpg";
            decimal price = 1.99m;
            string description = "Fresh apple";

            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, price, description, 0));
            Assert.Throws<ArgumentException>(() => new ProductAdd(name, imageUrl, price, description, -1));
        }
    }
}
