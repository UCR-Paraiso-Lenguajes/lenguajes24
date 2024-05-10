using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using storeApi.Models;
using storeApi.Database;
using storeApi.Business;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public  Store GetStore()
        {
            return Store.Instance ;
        }
    }

}