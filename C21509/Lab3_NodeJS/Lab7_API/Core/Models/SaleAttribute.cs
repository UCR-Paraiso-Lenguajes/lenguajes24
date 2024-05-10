using System;

namespace Store_API.Models
{
    public class SaleAttribute
    {
        public int SaleId { get; set; }
        public string PurchaseNumber { get; set; }
        public decimal Total { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Product { get; set; }
        public string DailySale { get; set; }
        public int SaleCounter { get; set; }

        public SaleAttribute(int saleId, string purchaseNumber, decimal total, DateTime purchaseDate, string product, string dailySale, int saleCounter)
        {
            if (saleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(saleId), "El ID de venta debe ser mayor que cero.");
            }
            SaleId = saleId;

            PurchaseNumber = purchaseNumber ?? throw new ArgumentNullException(nameof(purchaseNumber), "El número de compra no puede ser nulo.");

            if (total <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total), "El total de la venta debe ser mayor que cero.");
            }
            Total = total;

            if (purchaseDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(purchaseDate), "La fecha de compra no puede estar en el futuro.");
            }
            PurchaseDate = purchaseDate;

            Product = product ?? throw new ArgumentNullException(nameof(product), "El nombre del producto no puede ser nulo.");

            if (string.IsNullOrEmpty(dailySale))
            {
                throw new ArgumentNullException(nameof(dailySale), "El día de venta no puede ser nulo o vacío.");
            }
            DailySale = dailySale;

            if (saleCounter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(saleCounter), "El contador de ventas debe ser mayor o igual que cero.");
            }
            SaleCounter = saleCounter;
        }

        public SaleAttribute() { }

    }
}