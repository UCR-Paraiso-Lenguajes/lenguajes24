namespace storeApi.Models;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods.Type PaymentMethod { get;}
    public string PurchaseNumber { get; }

    public Sale(string purchaseNumber, IEnumerable<Product> products, string address, decimal amount, PaymentMethods.Type paymentMethod)
    {
        ValidateParameters(purchaseNumber, products, address, amount, paymentMethod);
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PurchaseNumber = purchaseNumber;
    }

    private void ValidateParameters(string purchaseNumber, IEnumerable<Product> products, string address, decimal amount, PaymentMethods.Type paymentMethod)
    {
        if (string.IsNullOrEmpty(purchaseNumber)){throw new ArgumentException("El número de compra no puede estar vacío o nulo.", nameof(purchaseNumber));}
        if (products == null || !products.Any()){throw new ArgumentException("La lista de productos no puede estar vacía.", nameof(products));}
        if (string.IsNullOrEmpty(address)){throw new ArgumentException("La dirección no puede estar vacía o nula.", nameof(address));}
        if (!Enum.IsDefined(typeof(PaymentMethods.Type), paymentMethod)){throw new ArgumentException("Método de pago no válido.", nameof(paymentMethod));}
        if (amount <= 0){throw new ArgumentException("El monto debe ser mayor que cero.", nameof(amount));}
    }

}
public class SaleAnnotation
{
    public DayOfWeek DayOfWeek { private set;  get; }
    public decimal Total { private set;  get; }
    public SaleAnnotation(DayOfWeek dayOfWeek, decimal total)
    {
        if (dayOfWeek==null ){throw new ArgumentException($"El {nameof(dayOfWeek)} no puede estar vacío o indefinido");}
        if (total < 0){throw new ArgumentException($"El {nameof(total)} no puede ser negativo");}
        DayOfWeek = dayOfWeek;
        Total = total;
    }
}