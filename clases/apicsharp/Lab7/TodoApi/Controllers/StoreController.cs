using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TodoApi.models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet, Authorize(Roles = "Customer")]
        public Store GetStore()
        {
            return Store.Instance ;
        }
    }

}
