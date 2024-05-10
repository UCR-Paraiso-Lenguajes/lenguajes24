using System.Security.Cryptography.X509Certificates;
using Core;
using StoreAPI.Business;
using StoreAPI.models;


namespace UT
{

  public class TestsStoreLogic
  {

    private StoreLogic storeLogic;

    [SetUp]
    public async Task SetupAsync()
    {
      string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
      Storage.Init(connectionString);
      storeLogic = new StoreLogic();
    }

    //Compra con carrito vacío:
    [Test]
    public void PurchaseAsync_EmptyCart_ThrowsArgumentException()
    {
      var cart = new Cart
      {
        ProductIds = new List<string>(),
        Address = "Dirección válida",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    //Compra con carrito nulo
    [Test]
    public void PurchaseAsync_NullCart_ThrowsArgumentException()
    {
      Cart cart = null;

      Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    // Compra sin dirección de envío
    [Test]
    public void PurchaseAsync_NoShippingAddress_ThrowsArgumentException()
    {
      var cart = new Cart
      {
        ProductIds = new List<string> { "1", "2" },
        Address = "", 
        PaymentMethod = PaymentMethods.Type.CASH 
      };

      Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    // Compra con lista de IDs de productos nula
    [Test]
    public void PurchaseAsync_NullProductIds_ThrowsArgumentException()
    {
      var cart = new Cart
      {
        ProductIds = null, 
        Address = "Dirección válida",
        PaymentMethod = PaymentMethods.Type.CASH 
      };

      Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    // Compra con lista de IDs de productos vacía
    [Test]
    public void PurchaseAsync_EmptyProductIds_ThrowsArgumentException()
    {
      var cart = new Cart
      {
        ProductIds = new List<string>(), 
        Address = "Dirección válida",
        PaymentMethod = PaymentMethods.Type.CASH 
      };

      Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    // Cálculo correcto del monto de la compra
    [Test]
    public async Task PurchaseAsync_CorrectPurchaseAmount()
    {

      var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Author= "Ana", ImgUrl= "a", Price = 6700 },
                new Product { Id = 2, Name = "Producto 2", Author= "Maria", ImgUrl= "a",  Price = 5800 }
            };

      var cart = new Cart
      {
        ProductIds = new List<string> { "1", "2" },
        Address = "Turrialba",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      var sale = await storeLogic.PurchaseAsync(cart);


      Assert.IsNotNull(sale);

      decimal price = (6700 * (1 + (decimal)Store.Instance.TaxPercentage / 100)) + (5800 * (1 + (decimal)Store.Instance.TaxPercentage / 100));
      decimal expectedPrice = (6700 + 5800) * (1 + (decimal)Store.Instance.TaxPercentage / 100);

      // Comprobar que el monto total de la venta sea igual al monto esperado
      Assert.AreEqual(21470, sale.Amount);
    }

    //Generación correcta del número de compra: 
    [Test]
    public async Task PurchaseAsync_GenerateCorrectPurchaseNumber()
    {
      var cart = new Cart
      {
        ProductIds = new List<string> { "1", "2" }, 
        Address = "Dirección válida",
        PaymentMethod = PaymentMethods.Type.CASH 
      };


      var sale = await storeLogic.PurchaseAsync(cart);

      Assert.IsNotNull(sale);
      Assert.IsTrue(IsValidPurchaseNumber(sale.NumberOrder)); 
    }

    //Manejo adecuado del método de pago
    [Test]
    public async Task PurchaseAsync_HandlePaymentMethodCorrectly()
    {

      var cart = new Cart
      {
        ProductIds = new List<string> { "1", "2" }, 
        Address = "Turrialba",
        PaymentMethod = PaymentMethods.Type.CASH 
      };

      var sale = await storeLogic.PurchaseAsync(cart);

      Assert.IsNotNull(sale);
      Assert.AreEqual(cart.PaymentMethod, sale.PaymentMethod);
    }

    //Happy path
    [Test]
    public async Task PurchaseAsync_SuccessfulPurchase()
    {
      // Arrange
      var cart = new Cart
      {
        ProductIds = new List<string> { "1", "2" },
        Address = "Turrialba",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      // Act
      var sale = await storeLogic.PurchaseAsync(cart);

      // Assert
      Assert.IsNotNull(sale);
      Assert.AreEqual("Turrialba", sale.Address);
      Assert.AreEqual(2, sale.Products.Count());
      Assert.AreEqual(21470, sale.Amount);
      Assert.AreEqual(PaymentMethods.Type.CASH, sale.PaymentMethod); 
      Assert.IsTrue(sale.Amount > 0); 
      Assert.IsFalse(String.IsNullOrEmpty(sale.NumberOrder));
    }


    // Método de ayuda para validar el número de compra
    private bool IsValidPurchaseNumber(string purchaseNumber)
    {
      return !string.IsNullOrEmpty(purchaseNumber) && purchaseNumber.Length == 10;
    }

  }
}
