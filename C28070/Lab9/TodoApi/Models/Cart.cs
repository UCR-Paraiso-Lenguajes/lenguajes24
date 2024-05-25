using System;

namespace TodoApi.Models
{
    public sealed class Cart
    {
        public required List<string> ProductIds { get; set; }
        public required string Address { get; set; }
        public PaymentMethods.Type PaymentMethod { get; set; } 
 
    }
}