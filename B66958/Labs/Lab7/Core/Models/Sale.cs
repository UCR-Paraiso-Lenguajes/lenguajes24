namespace ApiLab7;

public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods PaymentMethod { get; }
    public string PurchaseNumber { get; }
    public string ConfirmationNumber { get; }
    public string SaleDate { get; }

    private Sale(
        IEnumerable<Product> products,
        string address,
        decimal amount,
        PaymentMethods paymentMethod,
        string purchaseNumber,
        string confirmationNumber
    )
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PurchaseNumber = purchaseNumber;
        ConfirmationNumber = confirmationNumber;
    }

    private Sale(string date, decimal amount, string purchaseNumber, IEnumerable<Product> products)
    {
        SaleDate = date;
        Amount = amount;
        PurchaseNumber = purchaseNumber;
        Products = products;
    }

    public static Sale Build(
        IEnumerable<Product> products,
        string address,
        decimal amount,
        PaymentMethods paymentMethod,
        string purchaseNumber,
        string confirmationNumber
    )
    {
        if (products.Count() == 0)
            throw new ArgumentException("Sale must contain at least one product.");
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address must be provided.");
        if (amount <= 0)
            throw new ArgumentException("The final amount should be above 0");
        if (paymentMethod == null)
            throw new ArgumentException("A payment method should be provided");
        if (string.IsNullOrWhiteSpace(purchaseNumber))
            throw new ArgumentException("A purchase number must be provided");
        if (paymentMethod.PaymentType == PaymentMethods.Type.SINPE && confirmationNumber == null)
            throw new ArgumentException("The confirmation number must be provided");

        return new Sale(
            products,
            address,
            amount,
            paymentMethod,
            purchaseNumber,
            confirmationNumber
        );
    }

    public static Sale BuildForReports(
        string date,
        decimal amount,
        string purchaseNumber,
        IEnumerable<Product> products
    )
    {
        if (string.IsNullOrWhiteSpace(date))
            throw new ArgumentNullException("A date must be provided for a sale in the report");
        if (amount <= 0)
            throw new ArgumentException("The amount of a sale should be above 0");
        if (string.IsNullOrWhiteSpace(purchaseNumber))
            throw new ArgumentNullException(
                "A purchase number should exist for a sale in the report"
            );
        if (products.Count() == 0)
            throw new ArgumentException("Every sale should contain at least one product");

        return new Sale(date, amount, purchaseNumber, products);
    }
}
