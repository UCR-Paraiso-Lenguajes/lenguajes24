using Core;
using Microsoft.AspNetCore.Mvc;
using KEStoreApi.Bussiness;
using System;
using System.Threading.Tasks;
using static KEStoreApi.SaleLogic;
using System.Globalization;

namespace KEStoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private SaleLogic saleLogic = new(); 

        [HttpGet] 
        public async Task<IActionResult> GetSaleAsync(String date)
       
        {
             DateTime selectDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (selectDate== default(DateTime)) 
            {
                return BadRequest("Se requiere una fecha válida en el cuerpo de la solicitud.");
            }

            ReportSales report = await saleLogic.GetReportSalesAsync(selectDate);
            
            return Ok(report);
        }
    }
}
