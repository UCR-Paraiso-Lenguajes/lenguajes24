using Microsoft.AspNetCore.Mvc;
using StoreAPI;
using System;
using System.Collections.Generic;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<Cart> Carts = new List<Cart>();

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            // Add the cart to the list
            Carts.Add(cart);
            //to do: insertar datos, generar # compra
            // Return the newly created cart
            return Ok(cart);
        }
    }

}
//Sale sale = new(1234AB);