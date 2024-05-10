using Microsoft.AspNetCore.Mvc;
using Store_API.Business;

namespace Store_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesReportController : ControllerBase
    {
        private readonly SaleReportLogic _saleReportLogic;

       public SalesReportController()
        {
            _saleReportLogic = new SaleReportLogic();
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetSalesReportAsync(DateTime date)
        {
            if (date == DateTime.MinValue || date > DateTime.Now)
            {
                return BadRequest("Invalid date. Date cannot be later than the current date.");
            }

            try
            {
                var salesReport = await _saleReportLogic.GenerateSalesReportAsync(date);
                return Ok(salesReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the sales report: {ex.Message}");
            }
        }
    }
}