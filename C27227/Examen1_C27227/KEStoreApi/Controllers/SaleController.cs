using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KEStoreApi.Bussiness;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static KEStoreApi.SaleLogic;
namespace KEStoreApi
{
    [Route("api/")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly SaleLogic _saleLogic;

        public SaleController(SaleLogic saleLogic)
        {
            _saleLogic = saleLogic ?? throw new ArgumentNullException(nameof(saleLogic));
        }

        [HttpGet("sale")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSaleAsync([FromQuery] string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return BadRequest("Se requiere una fecha válida en el cuerpo de la solicitud.");
            }

            if (!DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectDate))
            {
                return BadRequest("Formato de fecha inválido. Utilice el formato dd/MM/yyyy.");
            }

            try
            {
                ReportSales report = await _saleLogic.GetReportSalesAsync(selectDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Se produjo un error al procesar la solicitud: {ex.Message}");
            }
        }
    }
}
