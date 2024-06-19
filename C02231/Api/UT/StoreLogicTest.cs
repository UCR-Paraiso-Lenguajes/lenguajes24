using System.Security.Cryptography.X509Certificates;
using Core;
using StoreAPI.Business;
using StoreAPI.Database;
using StoreAPI.models;


namespace UT
{

  public class TestsStoreLogic
  {

    private StoreLogic storeLogic;
    private List<Category> categoryList;
    [SetUp]
    public async Task SetupAsync()
    {
      string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
      Storage.Init(connectionString);
      StoreDB.CreateMysql();
      storeLogic = new StoreLogic();
      categoryList = new List<Category>
        {
            new(1, "Fantasy"),
            new (2, "Romance"),
            new (3, "Science Fiction"),
            new (4, "Young Adult"),
            new (5, "Mystery"),
            new (6, "NonFiction"),
            new (7, "Fiction"),
            new (8, "Adventure"),
            new (9, "Dystopian"),
            new (10, "Gift")
        };

    }

    //Compra con carrito vacío:
    [Test]
    public void PurchaseAsync_EmptyCart_ThrowsArgumentException()
    {
      var cart = new Cart
      {
        ProductIds = new List<ProductQuantity>(),
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
      Assert.ThrowsAsync<NullReferenceException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    // Compra sin dirección de envío
    [Test]
    public void PurchaseAsync_NoShippingAddress_ThrowsArgumentException()
    {
      var cart = new Cart
      {
        ProductIds = new List<ProductQuantity>{
        new ProductQuantity("1", 1), // Por ejemplo, asumiendo que el identificador del primer producto es "1" y la cantidad es 1
        new ProductQuantity("2", 1)  // Por ejemplo, asumiendo que el identificador del segundo producto es "2" y la cantidad es 1
        },
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
        ProductIds = new List<ProductQuantity>(),
        Address = "Dirección válida",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    // Cálculo correcto del monto de la compra
    [Test]
    public async Task PurchaseAsync_CorrectPurchaseAmount()
    {

      var storeInstance = await Store.Instance.Value;


      var products = new List<Product>
            {
                new Product ( "Producto 1", "Ana", "a", 6700, categoryList.Single(category => category.IdCategory == 8),1 ),
                new Product ("Producto 2", "Maria", "a", 5800,  categoryList.Single(category => category.IdCategory == 8), 1)
            };

      var cart = new Cart
      {
        ProductIds = new List<ProductQuantity>{
        new ProductQuantity("1", 1), // Por ejemplo, asumiendo que el identificador del primer producto es "1" y la cantidad es 1
        new ProductQuantity("2", 1)  // Por ejemplo, asumiendo que el identificador del segundo producto es "2" y la cantidad es 1
       },
        Address = "Turrialba",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      var sale = await storeLogic.PurchaseAsync(cart);


      Assert.IsNotNull(sale);

      decimal price = (6700 * (1 + (decimal)storeInstance.TaxPercentage / 100)) + (5800 * (1 + (decimal)storeInstance.TaxPercentage / 100));
      decimal expectedPrice = (6700 + 5800) * (1 + (decimal)storeInstance.TaxPercentage / 100);


      // Comprobar que el monto total de la venta sea igual al monto esperado
      Assert.That(sale.Amount, Is.EqualTo(19000));
    }

    //Generación correcta del número de compra: 
    [Test]
    public async Task PurchaseAsync_GenerateCorrectPurchaseNumber()
    {
      var cart = new Cart
      {
        ProductIds = new List<ProductQuantity>{
        new ProductQuantity("1", 1), // Por ejemplo, asumiendo que el identificador del primer producto es "1" y la cantidad es 1
        new ProductQuantity("2", 1)  // Por ejemplo, asumiendo que el identificador del segundo producto es "2" y la cantidad es 1
       },
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
        ProductIds = new List<ProductQuantity>{
        new ProductQuantity("1", 1), // Por ejemplo, asumiendo que el identificador del primer producto es "1" y la cantidad es 1
        new ProductQuantity("2", 1)  // Por ejemplo, asumiendo que el identificador del segundo producto es "2" y la cantidad es 1
       },
        Address = "Turrialba",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      var sale = await storeLogic.PurchaseAsync(cart);

      Assert.IsNotNull(sale);
      Assert.That(sale.PaymentMethod, Is.EqualTo(cart.PaymentMethod));
    }

    //Happy path
    [Test]
    public async Task PurchaseAsync_SuccessfulPurchase()
    {
      // Arrange
      var cart = new Cart
      {
        ProductIds = new List<ProductQuantity>{
        new ProductQuantity("1", 1), // Por ejemplo, asumiendo que el identificador del primer producto es "1" y la cantidad es 1
        new ProductQuantity("2", 1)  // Por ejemplo, asumiendo que el identificador del segundo producto es "2" y la cantidad es 1
       },
        Address = "Turrialba",
        PaymentMethod = PaymentMethods.Type.CASH
      };

      // Act
      var sale = await storeLogic.PurchaseAsync(cart);

      // Assert
      Assert.IsNotNull(sale);
      Assert.That(sale.Address, Is.EqualTo("Turrialba"));
      Assert.That(sale.Products.Count(), Is.EqualTo(2));
      Assert.That(sale.Amount, Is.EqualTo(19000));
      Assert.That(sale.PaymentMethod, Is.EqualTo(PaymentMethods.Type.CASH));
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
