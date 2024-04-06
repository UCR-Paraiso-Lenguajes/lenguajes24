using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace storeVetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public Store GetStore()
        {
            return Store.Instance ;
        }
    }

}
