namespace MyStoreAPI.Models
{
    public class Product: ICloneable
    {
        public string name { get; set; }
        public string imageUrl { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }        
        public string description { get; set; }        
        public decimal id { get; set; }


        public object Clone()
        {
            return new Product{
                id = this.id,
                name = this.name,
                imageUrl = this.imageUrl,
                price = this.price,
                quantity = this.quantity,                
                description = this.description
            };
        }
    }
}