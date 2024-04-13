using System.Text;

namespace StoreAPI.models;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods.Type PaymentMethod { get; }
    public string NumberOrder { get; }
    private static Random random = new Random();
 

    public Sale(IEnumerable<Product> products, string address, decimal amount, PaymentMethods.Type paymentMethod, string numberOrder)
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        NumberOrder = numberOrder;
    }

    internal static string GenerateNextPurchaseNumber()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder numberOrder = new StringBuilder();

        // Agrega 4 caracteres aleatorios
        for (int i = 0; i < 6; i++)
        {
            numberOrder.Append(chars[random.Next(chars.Length)]);
        }

        return numberOrder.ToString();
    }
}


