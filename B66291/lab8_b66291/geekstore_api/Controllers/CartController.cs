using Microsoft.AspNetCore.Mvc;
using geekstore_api.CardDb;

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
            var numeroCompra = sale.PurchaseNumber;
            data.almacenarDatos(sale);
            var response = new { numeroCompra = numeroCompra };
            return Ok(response);
            
        }
    }
}