using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KEStoreApi.Bussiness;
using KEStoreApi.Models;
using KEStoreApi.Data;
using Core;
using KEStoreApi;
using static KEStoreApi.Product;

namespace UnitTests
{
    public class StoreLogicTests
    {
        private StoreLogic _storeLogic;

        [SetUp]
        public void Setup()
        {
            var dbTest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(dbTest);
            DatabaseStore.Store_MySql();
            string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(connectionString);
            _storeLogic = new StoreLogic();
        }

        [Test]
        public void Purchase_WithEmptyCart_ThrowsArgumentException()
        {
            var cart = new Cart
            {
                Product = new List<ProductQuantity>(),
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    ZipCode = "12345",
                    Country = "AnyCountry"
                },
                PaymentMethod = PaymentMethods.Type.SINPE
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await _storeLogic.PurchaseAsync(cart));
        }

        [Test]
        public void Purchase_WithMissingAddress_ThrowsArgumentException()
        {
            var cart = new Cart
            {
                Product = new List<ProductQuantity>
                {
                    new ProductQuantity { Id = 1, Quantity = 2 },
                    new ProductQuantity { Id = 2, Quantity = 1 }
                },
                Address = null,
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
                    new ProductQuantity { Id = 1, Quantity = 2 }, 
                    new ProductQuantity { Id = 2, Quantity = 3 }
                },
                Address = new Address
                {
                    Street = "Cartago",
                    City = "Paraíso",
                    State = "State",
                    ZipCode = "12345",
                    Country = "Country"
                },
                PaymentMethod = PaymentMethods.Type.CASH
            };

            var sale = await _storeLogic.PurchaseAsync(cart);
            Assert.NotNull(sale);
            Assert.IsInstanceOf<Sale>(sale);
            Assert.That(sale.Products.Count(), Is.EqualTo(2)); 
            Assert.That(sale.Total, Is.EqualTo(1507.42));
            Assert.That(sale.Address, Is.EqualTo(cart.Address));
            Assert.That(sale.PaymentMethod, Is.EqualTo(PaymentMethods.Type.CASH));
            Assert.IsFalse(string.IsNullOrEmpty(sale.PurchaseNumber));
        }

        [Test]
        public async Task Purchase_Async_ProductsEmpty_ThrowsArgumentException()
        {
            var cart = new Cart
            {
                Product = new List<ProductQuantity>(),
                Address = new Address
                {
                    Street = "Cartago",
                    City = "Paraiso",
                    State = "State",
                    ZipCode = "12345",
                    Country = "Country"
                },
                PaymentMethod = PaymentMethods.Type.SINPE
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await _storeLogic.PurchaseAsync(cart));
        }
    }
}
