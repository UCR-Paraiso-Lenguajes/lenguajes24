using Microsoft.AspNetCore.Mvc;
using geekstore_api.DataBase;



namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
         private StoreLogic store = new StoreLogic(); 
         private CartDb data= new CartDb(); 

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            var sale = store.Purchase(cart); 
            data.procesarOrden(sale);
            var numeroCompra = sale.PurchaseNumber;
            var response = new { numeroCompra };
            return Ok(response);
            
        }
    }
}