namespace Core;

public class SaleDetails
{
    public string PurchaseNumber { get; private set; }
    public decimal Total { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public int Quantity { get; private set; }

    public string Product { get; private set; }

    public SaleDetails(string purchaseNumber, decimal total, DateTime purchaseDate, int quantity, string product)
    {
        PurchaseNumber = purchaseNumber;
        Total = total;
        PurchaseDate = purchaseDate;
        Quantity = quantity;
        Product = product;
    }
}
