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

        [Test]
        public void Purchase_WithNullCart_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => storeLogic.PurchaseAsync(null));
        }

        [Test]
        public async Task Purchase_WithInvalidAddress_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10)
            };
            Cart cart = new Cart(productQuantities, "", PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithNegativeTotal_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, -10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithNegativeSubtotal_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, 10, -10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithZeroTotalAndSubtotal_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 0)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, 0, 0);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithEmptyAddress_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10)
            };
            Cart cart = new Cart(productQuantities, "", PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithNullAddress_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10)
            };
            Cart cart = new Cart(productQuantities, null, PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithInvalidPaymentMethod_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", (PaymentMethods.Type)999, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithNegativeQuantity_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, -1, 10)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithZeroQuantity_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 0, 10)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithNegativePrice_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, -10)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithZeroPrice_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 0)
            };
            Cart cart = new Cart(productQuantities, "123 Main St", PaymentMethods.Type.CASH, 10, 10);

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithMultipleProducts()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 2, 15),
                new ProductQuantity(2, 1, 30)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                60,
                55
            );

            string mockPurchaseNumber = "JKL123";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }

        [Test]
        public async Task Purchase_WithSameProductMultipleTimes()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 15),
                new ProductQuantity(1, 1, 15)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.CASH,
                30,
                30
            );

            string mockPurchaseNumber = "MNO789";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }

        [Test]
        public async Task Purchase_WithNullProductList_ThrowsArgumentNullException()
        {
            Cart cart = new Cart(
                null,
                "123 Main St",
                PaymentMethods.Type.CASH,
                50,
                45
            );

            Assert.ThrowsAsync<ArgumentNullException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithTotalLessThanSubtotal_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 15)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.CASH,
                10,
                15
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithDifferentCurrencies_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10),
                new ProductQuantity(2, 1, 10000)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                10010,
                10005
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithValidDiscount()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                95,
                100
            );

            string mockPurchaseNumber = "QRS456";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }

        [Test]
        public async Task Purchase_WithInvalidDiscount_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                80,
                100
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithValidTax()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                105,
                100
            );

            string mockPurchaseNumber = "TUV123";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }

        [Test]
        public async Task Purchase_WithInvalidTax_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                120,
                100
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithValidPromoCode()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                90,
                100
            );

            string mockPurchaseNumber = "WXY789";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }

        [Test]
        public async Task Purchase_WithInvalidPromoCode_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                70,
                100
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithExpiredPromoCode_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 50),
                new ProductQuantity(2, 1, 50)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                85,
                100
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithDuplicateProducts()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10),
                new ProductQuantity(1, 2, 10)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                30,
                30
            );

            string mockPurchaseNumber = "ZAB123";
            Func<Sale, Task<string>> insertSaleAsync = async sale =>
            {
                sale.PurchaseNumber = mockPurchaseNumber;
                return mockPurchaseNumber;
            };

            string purchaseNumber = await storeLogic.PurchaseAsync(cart);

            Assert.AreEqual(mockPurchaseNumber, purchaseNumber);
        }

        [Test]
        public async Task Purchase_WithZeroQuantityForAllProducts_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 0, 10),
                new ProductQuantity(2, 0, 20)
            };

            Cart cart = new Cart(
                productQuantities,
                "123 Main St",
                PaymentMethods.Type.SINPE,
                0,
                0
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_WithShortAddress_ThrowsArgumentException()
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>
            {
                new ProductQuantity(1, 1, 10),
                new ProductQuantity(2, 1, 20)
            };

            Cart cart = new Cart(
                productQuantities,
                "A",  // Invalid short address
                PaymentMethods.Type.SINPE,
                30,
                30
            );

            Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
        }
    }
}
