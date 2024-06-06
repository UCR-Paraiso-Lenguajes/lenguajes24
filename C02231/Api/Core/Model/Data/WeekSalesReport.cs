namespace StoreAPI.models;

public sealed class WeekSalesReport
{

    public decimal Total { get; set; }
    public string DayOfWeek { get; set; }

    public WeekSalesReport(string dayOfWeek, decimal total)
    {
        if (dayOfWeek == null) throw new ArgumentNullException($"nameof(dayOfWeek) cannot be null.");
        if (total < 0) throw new ArgumentException("The sale total is required.");

        DayOfWeek = dayOfWeek;
        Total = total;
    }
}




