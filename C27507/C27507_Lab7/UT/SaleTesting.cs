using Core;
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using NUnit.Framework;

namespace UT{
    
    [TestFixture]
    public class SaleTesting{

        private List<Product> someProductsFromDB;

        [SetUp]
        public void SetUp(){
            TestBase.EnsureInitialized();
            //var productsFromDB = DB.SelectProducts();
            var productsFromDB = Store.Instance.Products;
            someProductsFromDB = new List<Product>();
            someProductsFromDB.Add(productsFromDB.ElementAt(0));
            someProductsFromDB.Add(productsFromDB.ElementAt(1));
        }



        //Test para los datos de consultas de ventas
        [Test]
        public async Task SaleDaaTesting(){

            var cartTest = new Cart{
                allProduct = someProductsFromDB,
                Subtotal = 1,
                Total = 1,
                Direction = "Costa Rica",
                PaymentMethod = PaymentMethods.paymentMethodsList.First(p => p.payment == PaymentMethodNumber.CASH)
            };

            //Asumimos que los datos del carrito se procesan como una venta
            SaleLogic saleLogic = new SaleLogic();            
            Sale saleConfirmed = await saleLogic.createSaleAsync(cartTest);
            var purchaseNum = saleConfirmed.purchaseNum;
            
            Assert.NotNull(saleConfirmed);
            Assert.NotNull(saleConfirmed.purchaseNum);
            Assert.AreEqual(cartTest, saleConfirmed.cart);
        }
    }
}