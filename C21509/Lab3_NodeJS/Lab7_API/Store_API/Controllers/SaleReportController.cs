using Microsoft.AspNetCore.Mvc;
using Store_API.Business;
using Microsoft.AspNetCore.Authorization;

namespace Store_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesReportController : ControllerBase
    {

        public SalesReportController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesReportAsync(DateTime date)
        {
            try
            {
                // Validación de la fecha si es necesario
                if (date == DateTime.MinValue || date > DateTime.Now)
                {
                    return BadRequest("Invalid date. Date cannot be later than the current date.");
                }

                var saleReportLogic = new SaleReportLogic();
                var salesReport = await saleReportLogic.GenerateSalesReportAsync(date);

                return Ok(salesReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the sales report: {ex.Message}");
            }
        }
    }
}