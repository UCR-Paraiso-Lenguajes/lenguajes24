namespace MyStoreAPI.Models
{
    public sealed class CartNoCategory
    {
        public List<ProductWithoutCategory> allProduct { get; set; }
        public decimal Subtotal {get;set;}
        public decimal Tax {get;set;} //revisar si debo hacerle un clone para mantener el impuesto de la venta en historia
        public decimal Total {get;set;}
        public string Direction {get;set;}
        public PaymentMethod PaymentMethod { get; set; }
        
    }
}