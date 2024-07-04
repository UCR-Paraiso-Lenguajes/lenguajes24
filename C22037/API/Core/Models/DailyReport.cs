using System;

namespace TodoApi.Models
{
    public sealed class DailyReport
    {
        public DateTime PurchaseDate { get; }
        public string PurchaseNumber { get; }
        public decimal Total { get; }

        public DailyReport(DateTime purchaseDate, string purchaseNumber, decimal total)
        {
            if (purchaseNumber == null) throw new ArgumentNullException("PurchaseNumber cannot be null.");
            if (total < 0) throw new ArgumentOutOfRangeException("Total must be positive.");

            PurchaseDate = purchaseDate;
            PurchaseNumber = purchaseNumber;
            Total = total;
        }
    }
}