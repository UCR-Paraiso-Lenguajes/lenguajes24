using System.Text.Json.Serialization;
using MyStoreAPI.Models;
namespace MyStoreAPI.Models
{
    public class ProductWithoutCategory: ICloneable
    {
        // Mantengo los set como público, ya que en los lugares donde se construyen instancias de Product
        // es más rápido y legible hacerlo sin constructor, asignando solo los valores a las variables sin orden estricto
        public string name { get; set; }
        public string imageUrl { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }        
        public string description { get; set; }        
        public decimal id { get; internal set; }

        public object Clone()
        {
            return new ProductWithoutCategory{
                id = this.id,
                name = this.name,
                imageUrl = this.imageUrl,
                price = this.price,
                quantity = this.quantity,                
                description = this.description,
                category = this.category
            };
        }
    }
}