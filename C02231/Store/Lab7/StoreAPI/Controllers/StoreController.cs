using Microsoft.AspNetCore.Mvc;
using StoreAPI.models;
using System;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {

        [HttpGet]
        public async Task<Store> GetStoreAsync()
        {
            var storeInstance = await Store.Instance.Value;
            return storeInstance;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductsByCategoriesAsync([FromQuery] IEnumerable<int> categories)
        {
            if (categories == null)
                throw new ArgumentException($"The {nameof(categories)} cannot be null");

            if (Products.Instance == null)
            {
                await Products.InitializeInstanceAsync();
            }

            var products = await Products.Instance.GetProductsCategoryAsync(categories);

            return Ok(products);
        }



        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] IEnumerable<int> categories, [FromQuery] string keywords)
        {
            // Validar las categorías
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    if (category < 0) throw new ArgumentException($"The list of {nameof(categories)} can't be null or empty.");
                }
            }

            // Validar que al menos uno de los parámetros (categorías o keywords) esté presente
            if ((categories == null || !categories.Any()) && string.IsNullOrWhiteSpace(keywords))
            {
                throw new ArgumentException("At least one category or keyword must be provided.");
            }

            try
            {
                IEnumerable<Product> products;

                // Si se proporcionan tanto categorías como palabras clave, buscar productos que coincidan con ambas
                if (categories != null && categories.Any() && !string.IsNullOrWhiteSpace(keywords))
                {
                    products = await Products.Instance.SearchProductsAsync(categories, keywords);

                }
                //Si solo se proporcionan categorías
                else if (categories != null && categories.Any())
                {
                    products = await Products.Instance.GetProductsCategoryAsync(categories);

                }
                // Si solo se proporcionan palabras clave
                else
                {
                    products = await Products.Instance.SearchProductsAsync(keywords);

                }

                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}
