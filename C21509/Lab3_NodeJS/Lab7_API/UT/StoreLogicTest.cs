using NUnit.Framework;
using Store_API.Business;
using Store_API.Models;
using Store_API.Database;

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
            List<int> productIds = new List<int>();
            string address = "123 Main St";
            PaymentMethods.Type paymentMethod = PaymentMethods.Type.SINPE;
            Cart cart = new Cart(productIds, address, paymentMethod, 0, 0);
            Assert.Throws<ArgumentException>(() => storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_HappyPath()
        {
            Cart cart = new Cart(
             new List<int> { 3, 4 }, 
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

