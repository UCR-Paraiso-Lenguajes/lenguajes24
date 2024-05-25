using System.Text;

namespace StoreAPI.models;
public sealed class SaleLine
{
    public Sale Sale { get; }
    public IEnumerable<Product> Products { get; }
    public decimal FinalPrice { get; }



    public SaleLine(Sale sale, IEnumerable<Product> products, decimal finalPrice)
    {
        Sale = sale;
        Products = products;
        FinalPrice = finalPrice;
    }
}