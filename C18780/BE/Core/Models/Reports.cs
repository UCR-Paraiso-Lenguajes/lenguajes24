using StoreApi.Models;

public sealed class Reports
{
    public IEnumerable<DailySales> dailySalesList { get; set; }
    public IEnumerable<WeeklySales> weeklySalesList { get; set; }

    public Reports(IEnumerable<DailySales> dailySalesList, IEnumerable<WeeklySales> weeklySalesList)
    {
        this.dailySalesList = dailySalesList;
        this.weeklySalesList = weeklySalesList;
    }
}