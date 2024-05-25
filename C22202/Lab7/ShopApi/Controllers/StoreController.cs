using Microsoft.AspNetCore.Mvc;
using ShopApi.Models;

namespace ShopApi.Controllers
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
        public IEnumerable<Product> GetCategories([FromQuery] List<int>? category, string? search)
        {
            if(category == null) category = new List<int>();
            if(category.Count == 0) category.Add(0);
            if(search == null) search = "";

            if(search != ""){
                return ProductsLogic.Instance.searchProducts(ProductsLogic.Instance.GetProductsCategory(category), search);
            }
            if (category.Count == 0) return ProductsLogic.Instance.GetProductsCategory(new List<int>{0});

            return ProductsLogic.Instance.GetProductsCategory(category);
        }

    }

}