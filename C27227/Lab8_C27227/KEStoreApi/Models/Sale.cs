using KEStoreApi;
using System;

namespace KEStoreApi;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get;}
    public PaymentMethods.Type PaymentMethod { get;}
    public  decimal amount;

    public string PurchaseNumber {get;}


    public Sale(string purcharseNumber, IEnumerable<Product> products, string address, decimal Amount, PaymentMethods.Type paymentMethod )
    {
        this.Products = products;
        this.Address = address;
        this.amount = Amount;
        this.PaymentMethod = paymentMethod;
        this.PurchaseNumber = purcharseNumber;
    }

}