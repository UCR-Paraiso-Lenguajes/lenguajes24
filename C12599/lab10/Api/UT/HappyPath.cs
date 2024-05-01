using NUnit.Framework;
using storeapi.Bussisnes;
using storeapi.Models;

namespace storeapi.UT
{
    [TestFixture]
    public class StoreLogicTests
    {
        [Test]
        public void Purchase_ValidCart_ReturnsSale()
        {
            // Arrange
            var storeLogic = new StoreLogic();
            var cart = new Cart
            {
                ProductIds = new List<string> { "1", "2", "3" }, // Simular IDs de productos en el carrito
                Address = "123 Main St",
                PaymentMethod = PaymentMethods.Type.CreditCard // Método de pago simulado
            };

            // Act
            var sale = storeLogic.Purchase(cart);

            // Assert
            Assert.IsNotNull(sale); // Verifica que se devuelve una venta (Sale) no nula
            Assert.IsNotEmpty(sale.Products); // Verifica que la lista de productos en la venta no esté vacía
            Assert.AreEqual(3, sale.Products.Count); // Verifica que se haya añadido la cantidad esperada de productos a la venta
            Assert.AreEqual("123 Main St", sale.Address); // Verifica que la dirección en la venta coincida con la dirección del carrito
            Assert.AreEqual(PaymentMethods.Type.CreditCard, sale.PaymentMethod); // Verifica que el método de pago en la venta sea el mismo que se pasó en el carrito
            Assert.Greater(sale.PurchaseAmount, 0); // Verifica que el monto de compra sea mayor que cero

            // Verifica que cada producto en la venta tenga un precio mayor que cero y coincida con el precio modificado por impuestos
            foreach (var product in sale.Products)
            {
                Assert.Greater(product.Price, 0); // Precio positivo
                Assert.AreEqual(product.Price, product.BasePrice * (1 + (decimal)Store.Instance.TaxPercentage / 100)); // Precio con impuestos aplicados
            }

            // Verifica que la venta se haya registrado correctamente en la base de datos
            var savedSale = CartSave.GetSaleFromDatabase(sale.Id);
            Assert.IsNotNull(savedSale); // Verifica que se haya encontrado la venta guardada en la base de datos
            Assert.AreEqual(sale.Id, savedSale.Id); // Verifica que el ID de la venta guardada coincida con el de la venta generada

            // Puedes agregar más aserciones según sea necesario para validar otros aspectos de la venta generada
        }
    }
}
