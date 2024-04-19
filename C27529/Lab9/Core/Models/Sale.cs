

using storeApi.Business;

namespace storeApi.Models;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethod.Type PaymentMethod { get; set; }
    public string PurchaseNumber { get; }


    public Sale(IEnumerable<Product> products, string address, decimal amount, PaymentMethod.Type paymentMethod)
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PurchaseNumber = StoreLogic.GenerateNextPurchaseNumber();
    }
}