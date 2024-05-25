using NUnit.Framework;
using storeapi.Bussisnes;
using storeapi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace storeapi.UT
{
    [TestFixture]
    public class StoreLogicTests
    {
        [Test]
        public async Task Purchase_ValidCart_ReturnsSale()
        {
            // Arrange
            var storeLogic = new StoreLogic();
            var cart = new Cart
            {
                ProductIds = new List<string> { "1", "2", "3" },
                Address = "123 Main St",
                PaymentMethod = PaymentMethods.Type.CreditCard
            };

            // Act
            var sale = await storeLogic.PurchaseAsync(cart);

            // Assert
            Assert.IsNotNull(sale);
            Assert.IsNotEmpty(sale.Products);
            Assert.AreEqual(3, sale.Products.Count);
            Assert.AreEqual("123 Main St", sale.Address);
            Assert.AreEqual(PaymentMethods.Type.CreditCard, sale.PaymentMethod);
            Assert.Greater(sale.PurchaseAmount, 0);

            foreach (var product in sale.Products)
            {
                Assert.Greater(product.Price, 0);
                Assert.AreEqual(product.Price, product.BasePrice * (1 + (decimal)Store.Instance.TaxPercentage / 100));
            }

            var savedSale = CartSave.GetSaleFromDatabase(sale.Id);
            Assert.IsNotNull(savedSale);
            Assert.AreEqual(sale.Id, savedSale.Id);
        }

        [Test]
        public void Purchase_NullCart_ThrowsArgumentNullException()
        {
            // Arrange
            var storeLogic = new StoreLogic();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await storeLogic.PurchaseAsync(null));
        }

        [Test]
        public void Purchase_EmptyProductIds_ThrowsArgumentException()
        {
            // Arrange
            var storeLogic = new StoreLogic();
            var cart = new Cart
            {
                ProductIds = new List<string>(), // Empty product IDs
                Address = "123 Main St",
                PaymentMethod = PaymentMethods.Type.CreditCard
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public void Purchase_NullAddress_ThrowsArgumentException()
        {
            // Arrange
            var storeLogic = new StoreLogic();
            var cart = new Cart
            {
                ProductIds = new List<string> { "1", "2", "3" },
                Address = null, // Null address
                PaymentMethod = PaymentMethods.Type.CreditCard
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }
    }
}
        

