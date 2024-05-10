using StoreAPI.models;
using StoreAPI.Database;

namespace StoreAPI.Business
{
    public sealed class SaleReportLogic
    {

        private readonly StoreDB storeDB;

        public SaleReportLogic()
        {
            storeDB = new StoreDB();
        }

        public async Task<SalesReport> GetSalesReportAsync(DateTime date)
        {
            if (date == DateTime.MinValue) throw new ArgumentException($"Invalid date provided: {nameof(date)}");
            StoreDB storeDB = new StoreDB();

            Task<IEnumerable<WeekSalesReport>> weeklySalesTask = storeDB.GetWeeklySalesAsync(date);
            Task<IEnumerable<DaySalesReports>> dailySalesTask = storeDB.GetDailySalesAsync(date);
            await Task.WhenAll(weeklySalesTask, dailySalesTask);

            IEnumerable<WeekSalesReport> weeklySales = await weeklySalesTask;
            IEnumerable<DaySalesReports> dailySales = await dailySalesTask;

            SalesReport salesReport = new SalesReport(dailySales, weeklySales);
            return salesReport;
        }

    }
    public class SalesReport
    {

        public IEnumerable<DaySalesReports> DailySales { get; set; }
        public IEnumerable<WeekSalesReport> WeeklySales { get; set; }
        public SalesReport(IEnumerable<DaySalesReports> dailySales, IEnumerable<WeekSalesReport> weeklySales)
        {
            if (dailySales == null || weeklySales == null) throw new ArgumentNullException("Parameters cannot be null");

            DailySales = dailySales;
            WeeklySales = weeklySales;
        }
    }
}
