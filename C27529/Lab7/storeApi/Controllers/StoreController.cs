using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using storeApi.Models;
using storeApi.Business;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public  ActionResult<Store>  GetStore()
        {
            return Ok(Store.Instance) ;
        }
    }

}