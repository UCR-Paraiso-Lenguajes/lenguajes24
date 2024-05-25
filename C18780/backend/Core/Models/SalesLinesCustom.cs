using StoreApi.utils;

namespace StoreApi.Models
{
    public sealed class SalesLinesCustom
    {
        public string NameProduct { get; set; }
        public decimal Subtotal { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}