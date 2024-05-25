using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using StoreApi.Models;


namespace StoreApi.Controllers
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