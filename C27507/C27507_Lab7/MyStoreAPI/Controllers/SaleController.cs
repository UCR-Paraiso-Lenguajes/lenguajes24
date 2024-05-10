using Core;
using Microsoft.AspNetCore.Mvc;

//API
using MyStoreAPI.Business;
using MyStoreAPI.Models;
namespace MyStoreAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class SaleController: ControllerBase{
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> GetSaleAsync([FromBody] DateTime dateFormat){
            try{                                
                SaleLogic saleLogic = new SaleLogic();
                RegisteredSaleReport specificListOfRegisteredSales = await saleLogic.getSalesByDayAndWeekAsync(dateFormat);                
                return Ok(new { specificListOfRegisteredSales });
            }
            //501 son para NotImplemented o Excepciones Propias
            catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }
            
        }

    }    
}

