using Microsoft.AspNetCore.Mvc;
using storeapi.Models;
using System;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStore()
        {
            Store storeInstance = Store.Instance;

            if (storeInstance == null)
            {
                throw new InvalidOperationException("No se pudo obtener la instancia de la tienda.");
            }
            return Ok(storeInstance);
        }
    }
}
