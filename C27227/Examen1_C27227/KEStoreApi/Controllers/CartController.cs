using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using KEStoreApi.Bussiness;

namespace KEStoreApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private StoreLogic storeLogic = new StoreLogic();

        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart)
        {

            if (cart == null)
            {
                return BadRequest("El objeto Cart no puede ser nulo.");
            }

            bool isCartEmpty = cart.Product == null || cart.Product.Count == 0;
            if (isCartEmpty)
            {
                return BadRequest("El carrito debe contener al menos un producto.");
            }

            // Realizar la compra
            var saleTask = storeLogic.PurchaseAsync(cart);
            var sale = await saleTask;
            var purchaseNumber = sale.PurchaseNumber;
            var response = new { purchaseNumber = purchaseNumber };
            return Ok(response);
        }
    }
}
