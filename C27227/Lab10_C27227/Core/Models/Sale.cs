using KEStoreApi;
using System;
using System.Collections.Generic;

public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public PaymentMethods.Type PaymentMethod { get; }
    public decimal Total { get; }
    public string PurchaseNumber { get; }

    public Sale(string purchaseNumber, IEnumerable<Product> products, string address, decimal total, PaymentMethods.Type paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(purchaseNumber)) { throw new ArgumentException($"El {nameof(purchaseNumber)} no puede ser nulo ni vacío."); }

        if (products == null || !products.Any()) { throw new ArgumentException($"La {nameof(products)} no puede ser nula ni vacía." );}
        
        if (string.IsNullOrWhiteSpace(address)) { throw new ArgumentException($"La {nameof(address)} no puede ser nula ni vacía.");}

        if (total <= 0) { throw new ArgumentException("El monto debe ser mayor que cero.", nameof(total)); }

        this.Products = products;
        this.Address = address;
        this.Total = total;
        this.PaymentMethod = paymentMethod;
        this.PurchaseNumber = purchaseNumber;
    }
}
