using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
         public async Task<IActionResult> GetStore()
        {
            var store = await Store.Instance;
            return Ok(new
            {
                Products = store.Products,
                TaxPercentage = store.TaxPercentage
            });
        }
    }

}