namespace Core;

public class SaleDetails
{
    public string PurchaseNumber { get; set; }
    public decimal Total { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int Quantity { get; set; }

    public string Product { get; set; }
}
