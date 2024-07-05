using System;
using System.Collections.Generic;
using Core;

namespace KEStoreApi
{
    public sealed class SaleLogic
    {
        private DatabaseSale _databaseSale = new();

        public SaleLogic()
        {
        }

        public SaleLogic(DatabaseSale databaseSale)
        {
            _databaseSale = databaseSale ?? throw new ArgumentNullException(nameof(databaseSale));
        }

        public async Task<ReportSales> GetReportSalesAsync(DateTime date)
        {
            if (date == DateTime.MinValue) { throw new ArgumentException($"La fecha no puede ser {nameof(date)}"); }
            if (date > DateTime.Now) { throw new ArgumentOutOfRangeException(nameof(date), "La fecha no puede ser posterior a la fecha actual."); }

            Task<IEnumerable<SaleDetails>> dailySalesTask = _databaseSale.GetDailySalesReportAsync(date);
            Task<IEnumerable<SalesByDay>> weeklySalesTask = _databaseSale.GetWeeklySalesReportAsync(date);
            await Task.WhenAll(dailySalesTask, weeklySalesTask);

            IEnumerable<SaleDetails> salesDetails = await dailySalesTask;
            IEnumerable<SalesByDay> salesByDays = await weeklySalesTask;
            ReportSales salesReport = new ReportSales(date, salesDetails, salesByDays);

            return salesReport;
        }

        public class ReportSales
        {
            public DateTime Date { get; private set; }
            public IEnumerable<SaleDetails> Sales { get; private set; }
            public IEnumerable<SalesByDay> SalesByWeek { get; private set; }

            public ReportSales(DateTime date, IEnumerable<SaleDetails> sales, IEnumerable<SalesByDay> salesByWeek)
            {
                Date = date;
                Sales = sales;
                SalesByWeek = salesByWeek;
            }
        }
    }
}
