using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public Store GetStore()
        {
            return Store.Instance ;   //define lo que va a retornar el metodo get
        }
    }

}