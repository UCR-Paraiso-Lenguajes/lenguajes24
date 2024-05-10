using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks; // Asegúrate de importar este namespace
using storeApi.Database;

namespace TodoApi.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        public class WeekDailyDate
        {
            public DateTime WeekDate { get; set; }
            public DateTime DailyDate { get; set; }
        }

        private class CombinedSalesData
        {
            public Dictionary<string, decimal> WeekSales { get; set; }
            public List<(string purchaseNumber, decimal total)> DailySales { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> GetSale([FromBody] WeekDailyDate dateString)
        {
            if (dateString.WeekDate == DateTime.MinValue)
            {
                return BadRequest("Invalid start date format");
            }

            if (dateString.DailyDate == DateTime.MinValue)
            {
                return BadRequest("Invalid today date.");
            }

            SaleDB saleDB = new SaleDB();
            
            Task<Dictionary<string, decimal>> weekSalesTask = saleDB.getWeekSalesAsync(dateString.WeekDate);
            Task<List<(string purchaseNumber, decimal total)>> dailySalesTask = saleDB.getDailySales(dateString.DailyDate);
            
            await Task.WhenAll(weekSalesTask, dailySalesTask);

            Dictionary<string, decimal> weekSales = await weekSalesTask;
            List<(string purchaseNumber, decimal total)> dailySales = await dailySalesTask;

            CombinedSalesData combinedSales = new CombinedSalesData
            {
                WeekSales = weekSales,
                DailySales = dailySales
            };

            return Ok(combinedSales);
        }
    }
}
