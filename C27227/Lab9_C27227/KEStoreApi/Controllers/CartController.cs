using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using KEStoreApi.Bussiness;

namespace KEStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
         private StoreLogic storeLogic = new StoreLogic();

         private readonly DatabaseSale dbSale = new DatabaseSale();         
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
           var sale = storeLogic.Purchase(cart);
           var purchaseNumber = sale.PurchaseNumber;
           try{
                dbSale.Save(sale);
                var response = new {purchaseNumber=purchaseNumber};
                return Ok(response);
           }catch(Exception ex){

            return StatusCode(500, $"Error al guardar la venta: {ex.Message}");
           }         
        }
    }

}