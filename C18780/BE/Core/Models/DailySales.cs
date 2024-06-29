using StoreApi.utils;

namespace StoreApi.Models
{
    public sealed class DailySales
    {
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public string NameProduct { get; set; }
        public decimal SubTotal { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
