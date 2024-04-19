using Microsoft.AspNetCore.Mvc;
using storeApi.Business;
using storeApi.DataBase;

using System;
using System.Collections.Generic;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private LogicStoreApi logicStore = new LogicStoreApi();
        private SaleDataBase saleDataBase = new SaleDataBase(); 
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            var sale = logicStore.Purchase(cart);
            var numeroCompra = sale.PurchaseNumber;
            try
            {
                saleDataBase.Save(sale);
                var response = new { numeroCompra = numeroCompra };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar la venta en la base de datos: {ex.Message}");
            }
        }
    }

}