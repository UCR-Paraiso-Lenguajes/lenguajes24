using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using System.Text;
namespace KEStoreApi.Bussiness;
public sealed class StoreLogic
{
    private static Random randomNumber = new Random();
    private DatabaseSale saleDataBase = new DatabaseSale(); 
    public StoreLogic()
    {

    }

public async Task<Sale> PurchaseAsync(Cart cart)
{
    if(cart == null){ throw new ArgumentNullException($"El objeto {nameof(cart)} no puede ser nulo.");}
    if (cart.Product.Count == 0) { throw new ArgumentException($"El {nameof(cart.Product.Count)} debe contener al menos un producto.");}
    if (string.IsNullOrWhiteSpace(cart.address)){throw new ArgumentException($"Se debe proporcionar una {nameof(cart.address)}");}

    var storeInstance = await Store.Instance;
    var products = storeInstance.Products;
    var taxPercentage = storeInstance.TaxPercentage;
    
    List<Product> matchingProducts = new List<Product>();
    decimal subtotal = 0;

    foreach (var productQuantity in cart.Product)
    {
        int productId = productQuantity.Id;
        int quantity = productQuantity.Quantity;


        Product matchingProduct = products.FirstOrDefault(p => p.Id == productId);
        if (matchingProduct == null){ throw new ArgumentException($"El {nameof(cart.Product)} con ID {productId} no existe en la tienda.");}

        if (quantity <= 0) { throw new ArgumentException($"La cantidad del {nameof(cart.Product)} con ID {productId} debe ser mayor que cero.");}


        Product copiedProduct = (Product)matchingProduct.Clone();


        copiedProduct.Quantity = quantity;

        matchingProducts.Add(copiedProduct);

        decimal totalPrice = quantity * matchingProduct.Price;

        subtotal += totalPrice;
    }


    decimal taxAmount = subtotal * ((decimal)taxPercentage / 100);
    decimal total = subtotal + taxAmount;

    PaymentMethods paymentMethod = PaymentMethods.SetPaymentType(cart.PaymentMethod);
    var purchase_number_Sale = GeneratePurchaseNumber();
    var sale = new Sale(purchase_number_Sale, matchingProducts, cart.address, total, paymentMethod.PaymentType);
    await saleDataBase.SaveAsync(sale);
    return sale;
}

    
    private string GeneratePurchaseNumber()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 6; i++)
        {
            int charType = randomNumber.Next(0, 3);
            switch (charType)
            {
                case 0: // Generar un dígito
                    sb.Append(randomNumber.Next(0, 10));
                    break;
                case 1: // Generar una letra mayúscula
                    sb.Append((char)randomNumber.Next('A', 'Z' + 1));
                    break;
                case 2: // Generar una letra minúscula
                    sb.Append((char)randomNumber.Next('a', 'z' + 1));
                    break;
            }
        }

        return sb.ToString();
    }

}