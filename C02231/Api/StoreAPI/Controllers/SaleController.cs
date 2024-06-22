using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Business;
using StoreAPI.Database;
using StoreAPI.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using static StoreAPI.Database.StoreDB;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {

        [HttpGet]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetSaleAsync([FromQuery] DateTime date)
        {
            try
            {
                if (date == DateTime.MinValue) throw new ArgumentException($"Invalid date provided: {nameof(date)}");

                SaleReportLogic saleLogic = new SaleReportLogic();

                SalesReport salesReport = await saleLogic.GetSalesReportAsync(date);
                return Ok(salesReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}