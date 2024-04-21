using System.ComponentModel.DataAnnotations;
using StoreApi.utils;

namespace StoreApi.Models
{
    public sealed class Sales
    {
        [Key]
        public Guid Uuid { get; set; }
        public DateTime Date { get; set; }
        public int Confirmation { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public string Address { get; set; }
    }
}