using Microsoft.AspNetCore.Mvc;
using StoreAPI;
using System;
using System.Collections.Generic;

namespace StoreAPI.Controllers
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
