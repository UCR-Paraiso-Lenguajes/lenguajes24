using Microsoft.AspNetCore.Mvc;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
namespace MyStoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]


    //cuando heredamos de ControllerBase, la clase ahora puede manejar solicitudes HTTP
    public class StoreController : ControllerBase{

        private StoreLogic storeLogic = new StoreLogic();
        
        //Enviar la tienda
        [HttpGet]
        public IActionResult getStore(){                        
            return Ok(Store.Instance);            
        }                
    }
}