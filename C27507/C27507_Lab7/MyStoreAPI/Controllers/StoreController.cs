using Microsoft.AspNetCore.Mvc;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

//JWT Authentication
using Microsoft.AspNetCore.Authorization;
namespace MyStoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]


    //cuando heredamos de ControllerBase, la clase ahora puede manejar solicitudes HTTP
    public class StoreController : ControllerBase{

        private StoreLogic storeLogic = new StoreLogic();
                
        //[HttpGet, Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getStore(){                        
            return Ok(Store.Instance);            
        }                
    }
}