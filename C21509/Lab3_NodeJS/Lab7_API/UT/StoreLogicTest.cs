using NUnit.Framework;
using Store_API.Business;
using Store_API.Models;
using Store_API.Database;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    public class StoreLogicTests
    {
        private StoreLogic storeLogic;

        [SetUp]
        public void Setup()
        {
            storeLogic = new StoreLogic();
        }

        [Test]
        public void Purchase_WithEmptyCart_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>();
            string address = "123 Main St";
            PaymentMethods.Type paymentMethod = PaymentMethods.Type.SINPE;
            Cart cart = new Cart(productQuantities, address, paymentMethod, 0, 0);
            Assert.Throws<ArgumentException>(() => storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_HappyPath()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(3, 1, 25),
                new ProductQuantity(4, 1, 25)
            };

            Cart cart = new Cart(
             productQuantities,
             "San José, Costa Rica",
             PaymentMethods.Type.CASH, 
             50, 
             100 
            );

            string mockPurchaseNumber = "FGH678";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }
    }
}

