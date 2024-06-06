using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using storeApi.Models;
using storeApi.Business;
using storeApi.Database;
using Microsoft.AspNetCore.Authorization;


namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {


        private StoreLogic storeLogic = new StoreLogic();
        private SaleDB saleDB = new SaleDB();
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> CreateCart([FromBody] Cart cart) // Debes hacer que el método sea asíncrono
        {

            if (cart.Address != null && cart.Address != "" && cart.Total >= 0 && cart.ProductIds.Count > 0)
            {
                var sale = await storeLogic.PurchaseAsync(cart); // Espera a que la tarea se complete
                var response = new { purchaseNumberResponse = sale.PurchaseNumber };
                return Ok(response);
            }
            else
            {
                throw new ArgumentException("Missing Information.");
            }
        }
    }

}