using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Database;

namespace TodoApi.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class saleController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetReportAsync([FromQuery] DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return BadRequest("Date parameter is required.");
            }

            if (date > DateTime.Now)
            {
                return BadRequest("Date cannot be in the future.");
            }

            SaleDB saleDB = new SaleDB();
            SalesReport salesReport = saleDB.GetSalesReport(date);
            return Ok(salesReport);
        }
    }
}