using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KEStoreApi.Bussiness;
using KEStoreApi;
using static KEStoreApi.Product;
using Core;
using Microsoft.VisualBasic;

namespace UnitTests
{
    public class StoreLogicTests
    {
        private StoreLogic _storeLogic;

        [SetUp]
        public void Setup()
        {
            string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(connectionString);
            _storeLogic = new StoreLogic();
        }

        [Test]
        public void Purchase_WithEmptyCart_ThrowsArgumentException()
        {
            Cart cart = new Cart
            {
                Product = new List<ProductQuantity>(),
                address = "123 Main St",
                PaymentMethod = PaymentMethods.Type.SINPE
            };
            
            Assert.ThrowsAsync<ArgumentException>(async () => await _storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public void Purchase_WithMissingAddress_ThrowsArgumentException()
        {
            Cart cart = new Cart
            {
            
                Product = new List<ProductQuantity>
                {
                    new ProductQuantity { Id = 1, Quantity = 2 },
                    new ProductQuantity { Id = 2, Quantity = 1 }
                },
                address = "",
                PaymentMethod = PaymentMethods.Type.CASH
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await _storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public async Task Purchase_HappyPath()
        {
            var cart = new Cart
            {
                Product = new List<ProductQuantity>
                {
                    new ProductQuantity {Id = 1, Quantity = 2 }, 
                    new ProductQuantity {Id =  2, Quantity = 3 }
                },
                address = "Cartago, Paraíso",
                PaymentMethod = PaymentMethods.Type.CASH 
            };

            var sale = await _storeLogic.PurchaseAsync(cart);
            Assert.NotNull(sale);
            Assert.IsInstanceOf<Sale>(sale);
            Assert.That(sale.Products.Count(), Is.EqualTo(2)); 
            Assert.That(sale.Total, Is.EqualTo(1507.42));
            Assert.That(sale.Address, Is.EqualTo(cart.address));
            Assert.That(sale.PaymentMethod, Is.EqualTo(PaymentMethods.Type.CASH));
            Assert.IsFalse(String.IsNullOrEmpty(sale.PurchaseNumber));
        }


    }
}
