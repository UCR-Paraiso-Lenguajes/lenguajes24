

namespace storeapi;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods.Type PaymentMethod { get; set; }
    public string PurchaseNumber { get; }


    public Sale(IEnumerable<Product> products, string address, decimal amount, PaymentMethods.Type paymentMethod)
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PurchaseNumber = StoreLogic.GenerateNextPurchaseNumber();
    }
}