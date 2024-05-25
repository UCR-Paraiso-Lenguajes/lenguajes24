using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public Store GetStore()
        {
            return Store.Instance;
        }
    }

}