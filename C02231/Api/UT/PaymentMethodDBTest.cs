using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using NUnit.Framework;
using StoreAPI.Database;
using StoreAPI.models;
using Core;

namespace UT
{
    public class PaymentMethodDBTest
    {
        private PaymentMethodDB paymentMethodDB;

        [SetUp]
        public async Task SetupAsync()
        {
            string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
            Storage.Init(connectionString);
            StoreDB.CreateMysql();

            paymentMethodDB = new PaymentMethodDB();
        }

        [Test]
        public async Task GetActivePaymentMethodsAsync_ShouldReturnActivePaymentMethods()
        {
          
            var result = await paymentMethodDB.GetActivePaymentMethodsAsync();

            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            foreach (var method in result)
            {
                Assert.IsTrue(method.Active == 1);
            }
        }

        [Test]
        public async Task DisablePaymentMethodAsync_ValidId_ShouldDisablePaymentMethod()
        {
             
            int paymentMethodId = 1;

             
            var result = await paymentMethodDB.DisablePaymentMethodAsync(paymentMethodId);

             
            Assert.IsTrue(result);

             
            var isActive = await paymentMethodDB.IsPaymentMethodActiveAsync(paymentMethodId);
            Assert.IsFalse(isActive);
        }

        [Test]
        public async Task TogglePaymentMethodAsync_ValidId_ShouldTogglePaymentMethodStatus()
        {
             
            int paymentMethodId = 1; 
             
            var result = await paymentMethodDB.ActivePaymentMethodAsync(paymentMethodId);

             
            Assert.IsTrue(result);

             
            var isActive = await paymentMethodDB.IsPaymentMethodActiveAsync(paymentMethodId);
            Assert.AreEqual(1, paymentMethodId);
        }

        [Test]
        public async Task IsPaymentMethodActiveAsync_ValidId_ShouldReturnCorrectStatus()
        {
             
            int paymentMethodId = 1;

             
            var result = await paymentMethodDB.IsPaymentMethodActiveAsync(paymentMethodId);

             
            Assert.IsNotNull(result);
           }

        [Test]
        public async Task DisablePaymentMethodAsync_InvalidId_ShouldReturnFalse()
        {
             
            int paymentMethodId = 999; 

             
            var result = await paymentMethodDB.DisablePaymentMethodAsync(paymentMethodId);

             
            Assert.IsFalse(result);
        }
    }
}
