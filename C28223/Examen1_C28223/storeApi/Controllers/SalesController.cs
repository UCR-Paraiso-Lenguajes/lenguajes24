using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using storeApi.Models;
using Microsoft.AspNetCore.Authorization;


namespace storeApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        internal readonly LogicSalesReportsApi lsr = new LogicSalesReportsApi();
        [HttpGet("sales/date"), Authorize(Roles = "Admin")]
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