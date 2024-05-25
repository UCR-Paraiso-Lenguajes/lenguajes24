using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KEStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
         public async Task<IActionResult> GetStoreAsync()
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