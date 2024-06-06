using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using Core;

//JWT Authentication
using Microsoft.AspNetCore.Authorization;

namespace MyStoreAPI.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase{                

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart){                        
            
            try{
                SaleLogic saleLogic = new SaleLogic();
                Sale saleConfirmed = await saleLogic.createSaleAsync(cart);
                var purchaseNum = saleConfirmed.purchaseNum;

                Console.WriteLine("Antes de mandar la respuesta post - Valor de saleConfirmed.purchaseNum: " + saleConfirmed.purchaseNum);
                return Ok(new { purchaseNum });                        
                 }
            catch (BussinessException){                
                return StatusCode(501, "Ha ocurrido un error al generar la transaccion. Por favor inténtalo más tarde.");
            }
            catch (Exception){                                             
                //Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al generar la transaccion. Por favor inténtalo más tarde.");
            }
        }        
    }
}