using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Models;
using StoreApi.utils;
using Microsoft.Extensions.Configuration;

namespace StoreApi.Repositories
{
    public class WeeklySalesRepository : IWeeklySalesRepository
    {
        private readonly IConfiguration _configuration;

        public WeeklySalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<WeeklySales>> GetWeeklySalesByDateAsync(DateTime dateTime)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var startDate = Utils.GetFirstDayOfTheWeek(dateTime);
                var endDate = startDate.AddDays(6);

                var weeklySalesList = await (from sales in dbContext.Sales
                                             where sales.Date >= startDate && sales.Date <= endDate
                                             group sales by new { sales.Date.Year, sales.Date.Month, sales.Date.Day } into groupedSales
                                             select new WeeklySales
                                             {
                                                 Date = groupedSales.Key.Year + "-" + groupedSales.Key.Month + "-" + groupedSales.Key.Day,
                                                 Total = groupedSales.Sum(x => x.Total)
                                             }).ToListAsync();

                return weeklySalesList;
            }
        }
    }
}
