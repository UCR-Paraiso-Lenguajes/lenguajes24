using System;
using System.Collections.Generic;
using NUnit.Framework;
using TodoApi.Models;

namespace TodoApi.Tests
{
    [TestFixture]
    public class CartModelTest
    {
        [Test]
        public void Constructor_ShouldInitializeProperties_WhenValidParametersAreProvided()
        {
            var productIds = new List<string> { "prod1", "prod2" };
            var address = "123 Main St";
            var paymentMethod = PaymentMethod.Type.SINPE;
            var total = 100.0m;
            var productQuantities = new Dictionary<string, int>
            {
                { "prod1", 1 },
                { "prod2", 2 }
            };

            var cart = new Cart(productIds, address, paymentMethod, total, productQuantities);

            Assert.AreEqual(productIds, cart.ProductIds);
            Assert.AreEqual(address, cart.Address);
            Assert.AreEqual(paymentMethod, cart.PaymentMethod);
            Assert.AreEqual(total, cart.Total);
            Assert.AreEqual(productQuantities, cart.ProductQuantities);
        }

        [Test]
        public void GetQuantityForProduct_ShouldReturnCorrectQuantity_WhenProductExists()
        {
            var productIds = new List<string> { "prod1", "prod2" };
            var address = "123 Main St";
            var paymentMethod = PaymentMethod.Type.SINPE;
            var total = 100.0m;
            var productQuantities = new Dictionary<string, int>
            {
                { "prod1", 1 },
                { "prod2", 2 }
            };
            var cart = new Cart(productIds, address, paymentMethod, total, productQuantities);

            var quantity = cart.GetQuantityForProduct("prod1");

            Assert.AreEqual(1, quantity);
        }

        [Test]
        public void GetQuantityForProduct_ShouldReturnZero_WhenProductDoesNotExist()
        {
            var productIds = new List<string> { "prod1", "prod2" };
            var address = "123 Main St";
            var paymentMethod = PaymentMethod.Type.SINPE;
            var total = 100.0m;
            var productQuantities = new Dictionary<string, int>
            {
                { "prod1", 1 },
                { "prod2", 2 }
            };
            var cart = new Cart(productIds, address, paymentMethod, total, productQuantities);

            var quantity = cart.GetQuantityForProduct("prod3");

            Assert.AreEqual(0, quantity);
        }
    }
}
