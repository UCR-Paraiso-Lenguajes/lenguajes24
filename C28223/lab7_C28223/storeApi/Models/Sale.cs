namespace storeApi;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods.Type PaymentMethod { get; }
    public string PurchaseNumber { get; }



    public Sale(string purchaseNumber, IEnumerable<Product> products, string address, decimal Amount, PaymentMethods.Type paymentMethod)
    {
        ValidateParameters(purchaseNumber, products, address, Amount, paymentMethod);
        this.Products = products;
        this.Address = address;
        this.Amount = Amount;
        this.PaymentMethod = paymentMethod;
        this.PurchaseNumber = purchaseNumber;
    }

    internal static string GenerateNextPurchaseNumber()
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Random random = new Random();
        string purchaseNumber = "";

        for (int i = 0; i < 6; i++)
        {
            purchaseNumber += chars[random.Next(chars.Length)];
        }
        return purchaseNumber;
    }

    private static void ValidateParameters(string purchaseNumber, IEnumerable<Product> products, string address, decimal amount, PaymentMethods.Type paymentMethod)
    {
        if (string.IsNullOrEmpty(purchaseNumber))
        {
            throw new ArgumentException("El número de compra no puede estar vacío o nulo.", nameof(purchaseNumber));
        }

        if (products == null || !products.Any())
        {
            throw new ArgumentException("La lista de productos no puede estar vacía.", nameof(products));
        }

        if (string.IsNullOrEmpty(address))
        {
            throw new ArgumentException("La dirección no puede estar vacía o nula.", nameof(address));
        }
        if (!Enum.IsDefined(typeof(PaymentMethods.Type), paymentMethod))
        {
            throw new ArgumentException("Método de pago no válido.", nameof(paymentMethod));
        }
        if (amount <= 0)
        {
            throw new ArgumentException("El monto debe ser mayor que cero.", nameof(amount));
        }
    }

}