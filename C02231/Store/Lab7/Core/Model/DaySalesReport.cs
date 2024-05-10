namespace StoreAPI.models;

public sealed class DaySalesReports
{
    public DateTime PurchaseDate { get; set; }
    public string PurchaseNumber { get; set; }
    public decimal Total { get; set; }

    public string Products { get; set; }

    public int Quantity { get; set; }

    public DaySalesReports(DateTime purchaseDate, string purchaseNumber, int quantity,decimal total, string products)
    {

        if (purchaseDate == DateTime.MinValue) throw new ArgumentException("Invalid date provided.", nameof(purchaseDate));
        if (total == null) throw new ArgumentException("The sale total is required.");
        if (products == null) throw new ArgumentException("The products are required.");
        if(quantity == 0) throw new ArgumentException("The quantity is required.");

        PurchaseDate = purchaseDate;
        PurchaseNumber = purchaseNumber;
        Quantity = quantity;
        Total = total;
        Products = products;
    }
}




