using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using storeApi.Database;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


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
            public required Dictionary<string, decimal> WeekSales { get; set; }
            public required Dictionary<string, decimal> DailySales { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> GetSale([FromBody] WeekDailyDate dateString)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            if (dateString.WeekDate == DateTime.MinValue || dateString.DailyDate == DateTime.MinValue)
            {
                return BadRequest("Invalid date format.");
            }


            SaleDB saleDB = new SaleDB();
            var weekSalesTask = saleDB.getWeekSalesAsync(dateString.WeekDate);
            var dailySalesTask = saleDB.getDailySales(dateString.DailyDate);


            await Task.WhenAll(weekSalesTask, dailySalesTask);

            var combinedSales = new CombinedSalesData
            {
                WeekSales = await weekSalesTask,
                DailySales = await dailySalesTask
            };


            return Ok(combinedSales);
        }

    }
}
