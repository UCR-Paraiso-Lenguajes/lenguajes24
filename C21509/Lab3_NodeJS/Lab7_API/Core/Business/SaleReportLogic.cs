using Store_API.Database;
using Store_API.Models;

namespace Store_API.Business
{
    public class SaleReportLogic
    {
        private readonly DB_API _dbApi;

        public SaleReportLogic()
        {
            
        }

        public async Task<SalesReport> GenerateSalesReportAsync(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                throw new ArgumentException("The date cannot be DateTime.MinValue.", nameof(date));
            }

            if (date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(date), "The date cannot be later than the current date.");
            }

            var dailySalesTask = _dbApi.ObtainDailySalesAsync(date);
            var weeklySalesTask = _dbApi.ObtainWeeklySalesAsync(date);

            await Task.WhenAll(dailySalesTask, weeklySalesTask);

            var dailySales = await dailySalesTask;
            var weeklySales = await weeklySalesTask;

            var salesReport = new SalesReport
            {
                Date = date,
                DailySales = dailySales,
                WeeklySales = weeklySales
            };

            return salesReport;
        }

        public class SalesReport
        {
            public DateTime Date { get; set; }
            public IEnumerable<SaleAttribute> DailySales { get; set; }
            public IEnumerable<SaleAttribute> WeeklySales { get; set; }

            public int CountDailySales()
            {
                return DailySales?.Count() ?? 0;
            }

            public int CountWeeklySales()
            {
                return WeeklySales?.Count() ?? 0;
            }
        }
    }
}