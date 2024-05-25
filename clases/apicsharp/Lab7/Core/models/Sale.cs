using System.Text;

namespace TodoApi.models;
public sealed class Sale
{
    private static Random random = new Random();
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public readonly decimal amount;
    public PaymentMethods PaymentMethod { get; }
    public string PurchaseNumber { get; }

    public decimal Amount{  get; }

    public Sale(string purchaseNumber, IEnumerable<Product> products, string address, decimal Amount, PaymentMethods paymentMethod )
    {
        this.Products = products;
        this.Address = address;
        this.amount = Amount;
        this.PaymentMethod = paymentMethod;
        this.PurchaseNumber = purchaseNumber;
    }

    internal static string GenerateNextPurchaseNumber()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder purchaseNumber = new StringBuilder();

        // Agrega 4 caracteres aleatorios
        for (int i = 0; i < 6; i++)
        {
            purchaseNumber.Append(chars[random.Next(chars.Length)]);
        }

        return purchaseNumber.ToString();
    }
}
