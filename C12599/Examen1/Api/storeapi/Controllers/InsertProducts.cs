using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeapi.Models;
using storeapi.Business;
using Microsoft.Extensions.Caching.Memory;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertProductsController : ControllerBase
    {
        private readonly InsertProductsLogic _insertProductsLogic;
        private readonly Categories _categories;

        public InsertProductsController(IMemoryCache cache, Categories categories)
        {
            _categories = categories;
            _insertProductsLogic = new InsertProductsLogic(cache, InsertProductToList);
        }

        [HttpPost]
        public IActionResult InsertProduct([FromBody] ProductRequestModel request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            Category category = _categories.GetCategoryById(request.CategoryId);
         

            Product product = new Product
            {
                id = request.Id,
                Name = request.Name,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                Description = request.Description,
                Category = category
            };

            var updatedProducts = _insertProductsLogic.InsertProduct(product);
            return Ok(updatedProducts);
        }

        private void InsertProductToList(Product product, List<Product> products)
        {
            products.Add(product);
        }
    }

    public class ProductRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
