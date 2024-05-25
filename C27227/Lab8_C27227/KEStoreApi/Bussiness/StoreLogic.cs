using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using System.Text;
namespace KEStoreApi.Bussiness;
public sealed class StoreLogic
{
    private static Random randomNumber = new Random();
    public StoreLogic()
    {

    }

public Sale Purchase(Cart cart)
    {
        bool isCartEmpty = cart.ProductID.Count == 0;
        bool isDeliveryAddressMissing = string.IsNullOrWhiteSpace(cart.address);
        
        if (isCartEmpty)
            throw new ArgumentException("Cart must contain at least one product.");
        if (isDeliveryAddressMissing)
            throw new ArgumentException("Address must be provided.");

        var products = Store.Instance.Products;
        var taxPercentage = Store.Instance.TaxPercentage;

        IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductID.Contains(p.Id.ToString())).ToList();

        IEnumerable<Product> deepCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        decimal purchaseAmount = 0;
        foreach (var product in deepCopyProducts)
        {
            product.Price *= (1 + (decimal)taxPercentage / 100);
            purchaseAmount += product.Price;
        }

        PaymentMethods paymentMethod = PaymentMethods.SetPaymentType(cart.PaymentMethod);

        var sale = new Sale(GeneratePurchaseNumber(), deepCopyProducts, cart.address, purchaseAmount, paymentMethod.PaymentType);
        
        return sale;
    }

    
    public static string GeneratePurchaseNumber()
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