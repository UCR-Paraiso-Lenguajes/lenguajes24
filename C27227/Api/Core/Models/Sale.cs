using KEStoreApi;
using KEStoreApi.Models;
using System;
using System.Collections.Generic;

public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public Address Address { get; }
    public PaymentMethods.Type PaymentMethod { get; }
    public decimal Total { get; }
    public string PurchaseNumber { get; }

    public Sale(string purchaseNumber, IEnumerable<Product> products, Address address, decimal total, PaymentMethods.Type paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(purchaseNumber)) 
        { 
            throw new ArgumentException($"El {nameof(purchaseNumber)} no puede ser nulo ni vacío."); 
        }

        if (products == null || !products.Any()) 
        { 
            throw new ArgumentException($"La {nameof(products)} no puede ser nula ni vacía."); 
        }

        if (address == null) 
        { 
            throw new ArgumentException($"El {nameof(address)} no puede ser nulo."); 
        }

        if (total <= 0) 
        { 
            throw new ArgumentException("El monto debe ser mayor que cero.", nameof(total)); 
        }

        this.Products = products;
        this.Address = address;
        this.Total = total;
        this.PaymentMethod = paymentMethod;
        this.PurchaseNumber = purchaseNumber;
    }
}
