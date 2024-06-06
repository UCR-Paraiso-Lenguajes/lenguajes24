using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using storeApi.Business;
using storeApi.Models;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        internal readonly LogicSalesReportsApi lsr = new LogicSalesReportsApi();

        [HttpGet("{date}")]
        public async Task<IActionResult> CreateReportSales(DateTime date)
        {
            try
            {
                SalesReport report = await lsr.GetSalesReportAsync(date);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}