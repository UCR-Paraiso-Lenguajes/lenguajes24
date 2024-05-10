using Microsoft.AspNetCore.Mvc;
using Store_API.Models;

namespace Store_API.Controllers
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