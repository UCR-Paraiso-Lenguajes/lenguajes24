using KEStoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KEStoreApi.Bussiness;

namespace KEStoreApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpPost("validate-address")]
        [AllowAnonymous]
        public IActionResult ValidateAddress([FromBody] Address address)
        {
            if (StoreLogic.IsValidAddress(address))
            {
                return Ok();
            }
            else
            {
                return BadRequest("La dirección de entrega no es válida.");
            }
        }
    }
}
