using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("Products")]
        public IEnumerable<Product> GetCategories(int category)
        {
            if (category < 1)
                throw new ArgumentException("The category number must be above 0");
            return Store.Instance.ProductsByCategory(category);
        }
    }
}
