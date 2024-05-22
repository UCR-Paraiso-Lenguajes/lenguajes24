using System;
using System.Collections.Generic;
using TodoApi.Business;
using TodoApi.Database;

namespace TodoApi.Models
{
    public sealed class DailyReport
    {
        public DateTime PurchaseDate { get; }
        public string PurchaseNumber { get; }
        public decimal Total { get; }

        public DailyReport(DateTime purchaseDate, string purchaseNumber,decimal total)
        {
            PurchaseDate = purchaseDate;
            PurchaseNumber = purchaseNumber;
            Total = total;
        }
    }
}