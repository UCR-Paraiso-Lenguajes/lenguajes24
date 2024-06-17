using System;
using System.Collections.Generic;

namespace storeApi.Models.Data
{
    public sealed class SalesData
    {
        public DateTime PurchaseDate { get;private set; }
        public string PurchaseNumber { get; private set; }
        public decimal Total { get; private set; }
        public int AmountProducts { get; set; }
        public IEnumerable<ProductQuantity> ProductsAnnotation { get; private set; }

        // Constructor que inicializa todas las propiedades de la clase
        public SalesData(DateTime purchaseDate, string purchaseNPurchaseNumber, decimal total, int amountProducts, List<ProductQuantity> productsAnnotation)
        {
            ValidateSalesData(purchaseNPurchaseNumber, purchaseDate, total, amountProducts, productsAnnotation); 
            PurchaseDate = purchaseDate;
            PurchaseNumber = purchaseNPurchaseNumber;
            Total = total;
            AmountProducts = amountProducts;
            ProductsAnnotation = productsAnnotation;
        }
        private void ValidateSalesData(string purchaseNPurchaseNumber, DateTime purchaseDate, decimal total, int amountProducts, List<ProductQuantity> productsAnnotation)
        {
            if (string.IsNullOrEmpty(purchaseNPurchaseNumber)){throw new ArgumentException($"El ID de compra no puede estar vacío o nulo, {nameof(purchaseNPurchaseNumber)}");}
            if (purchaseDate == default(DateTime)){throw new ArgumentException("La fecha de compra no puede ser la predeterminada.", nameof(purchaseDate));}
            if (total < 0){throw new ArgumentException("El total debe ser mayor que cero.", nameof(total));}
            if (amountProducts < 0){throw new ArgumentException($"{nameof(amountProducts)}de los productos debe ser mayor que cero.");}
        }
    }
}
