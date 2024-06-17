// InsertProductsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeapi.Models;
using storeapi.Business;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;

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
            _insertProductsLogic = new InsertProductsLogic(cache, InsertProductToDatabase);
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

        private static void InsertProductToDatabase(Product product, MySqlConnection connection, MySqlTransaction transaction)
        {
            string insertProductQuery = @"
                INSERT INTO products (name, price, description, image, category)
                VALUES (@name, @price, @description, @image, @category)";

            using (var insertCommand = new MySqlCommand(insertProductQuery, connection, transaction))
            {
                insertCommand.Parameters.AddWithValue("@name", product.Name);
                insertCommand.Parameters.AddWithValue("@price", product.Price);
                insertCommand.Parameters.AddWithValue("@description", product.Description);
                insertCommand.Parameters.AddWithValue("@image", product.ImageUrl);
                insertCommand.Parameters.AddWithValue("@category", product.Category.Id);
                insertCommand.ExecuteNonQuery();
            }
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
