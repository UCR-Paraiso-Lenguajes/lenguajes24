using NUnit.Framework;
using core;
using core.DataBase;
using core.Models;
using core.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UT;

public class PaymentMethodLogicTesting
{
private CampainBusiness campainBusiness;
private PaymentLogic payment = new PaymentLogic();

        [SetUp]
        public void Setup()
        {
           var myDbtest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            myDbtest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            StoreDb.CrearDatosSync();
            campainBusiness = new CampainBusiness();
        }

        [Test]
        public async Task ObtenerMetodosPago()
        {
            var paymentMethods = await payment.GetPaymentMethods();
            Assert.IsNotNull(paymentMethods);
            Assert.AreEqual(2, paymentMethods.Count());

        }

    


}
