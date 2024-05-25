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
           dbSale.save(sale);
            var response = new {purchaseNumber=purchaseNumber};
            return Ok(response);
        }
    }

}