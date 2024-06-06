namespace MyStoreAPI.Models{
    public sealed class SaleLine{

        public string  IdSale { get; set; }
        public string  IdProduct { get; set; }
        public decimal  Quantity { get; set; }
        public decimal  PricePaid { get; set; }
        public string  OriginalProductName { get; set; }
        public decimal OriginalProductPrice { get; set; }        
    }
}
