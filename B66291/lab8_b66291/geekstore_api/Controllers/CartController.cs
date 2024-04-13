using Microsoft.AspNetCore.Mvc;
using geekstore_api.CardDb;

namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
         private readonly CartDb data= new CartDb();
         private readonly PurchaseNumber number= new PurchaseNumber();
        //falta modificar

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            int numeroCompra = number.generarNumeroCompra();
            int paymentMethodValue = (int)cart.PaymentMethod;
            data.almacenarDatos(cart.total, DateTime.Now, numeroCompra, paymentMethodValue);
            return Ok(new { NumeroCompra = numeroCompra });
        }
    }

}