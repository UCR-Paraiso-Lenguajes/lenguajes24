using KEStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace KEStoreApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpPost("validate-address")]
        public IActionResult ValidateAddress([FromBody] Address address)
        {
            if (Store.Instance.Result.IsValidAddress(address))
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
