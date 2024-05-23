using Microsoft.AspNetCore.Mvc;
using core.Business;
using core.DataBase;
using core.Models;


namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
         private StoreLogic store = new StoreLogic(); 
         private CartDb data= new CartDb(); 

        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart)
        {
            if(cart == null){
                return BadRequest("El objeto no puede ser nulo");
            } 
            var sale = await store.PurchaseAsync(cart); 
			var numeroCompra = sale.PurchaseNumber; 
			var response = new { numeroCompra }; 
			return Ok(response);
		}
    }
}