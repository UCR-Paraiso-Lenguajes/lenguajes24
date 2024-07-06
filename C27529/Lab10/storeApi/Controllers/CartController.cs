using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using storeApi.Models;
using storeApi.Business;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private StoreLogic storeLogic = new StoreLogic();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            if (cart == null || cart.Address == null || cart.Address == "" || cart.Total < 0 || cart.ProductIds == null || cart.ProductIds.Count == 0)
            {
                return BadRequest("Missing Information.");
            }
            
            var sale = await storeLogic.PurchaseAsync(cart);
            var response = new { purchaseNumberResponse = sale.PurchaseNumber };
            return Ok(response);
        }
    }
}