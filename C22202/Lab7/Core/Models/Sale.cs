namespace ShopApi.Models;

public sealed class Sale
{
    public IEnumerable<Product> products { get; }
    public string address { get; }
    public decimal total { get; }
    public PaymentMethods.Type paymentMethod { get; }
    public string purchase_number {get;}

    public Sale(IEnumerable<Product> products, string address, decimal total, PaymentMethods.Type paymentMethod, string purchase_number)
    {
        this.products = products;
        this.address = address;
        this.total = total;
        this.paymentMethod = paymentMethod;
        this.purchase_number = purchase_number;
    }
}