using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Models
{
    public sealed class SalesLine
    {
        [Key]
        public Guid Uuid { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        [ForeignKey("Product")]
        public Guid UuidProduct { get; set; }
        [ForeignKey("Sales")]
        public Guid UuidSales { get; set; }
    }
}