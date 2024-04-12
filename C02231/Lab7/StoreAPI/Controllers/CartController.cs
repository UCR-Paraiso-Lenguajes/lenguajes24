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
        private static List<Cart> Carts = new List<Cart>();

        private readonly BDSale saveCart = new BDSale();

        //to do generar # compra
       
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {



            //to do: insertar datos

            string purchaseNumber = Sale.GenerateNextPurchaseNumber();
            int paymentMethod = (int)cart.PaymentMethods;
                 //sale.save(DateTime.Now, cart.Total, paymentMethod, purchaseNumber);

            

            // Add the cart to the list
            Carts.Add(cart);
            // Return the newly created cart
            return Ok(cart);
        }
    }

}