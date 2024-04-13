using Microsoft.AspNetCore.Mvc;
using StoreAPI;
using StoreAPI.Database;
using StoreAPI.models;
using System;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly BDSale saveSale = new();

       
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            string purchaseNumber = Sale.GenerateNextPurchaseNumber();
            int paymentMethod = (int)cart.PaymentMethod;
                
            saveSale.Save(DateTime.Now, cart.Total, paymentMethod,  purchaseNumber);

            
            return Ok(purchaseNumber);
        }
    }

}