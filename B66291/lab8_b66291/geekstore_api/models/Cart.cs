namespace geekstore_api;
public sealed class Cart //esto es lo que se envia en el post
{
    public List <string> ProductIds { get; set; }//lista de productos con sus respectivos id
    public string Address { get; set; }
    public PaymentMethods.Type PaymentMethod { get; set; }
    public decimal total {get; set;}
}