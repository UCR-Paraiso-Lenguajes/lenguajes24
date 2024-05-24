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

        [HttpGet("products")]
        public IEnumerable<Product> GetProductsCategories(
            [FromQuery(Name = "categories")] List<int> categories = null,
            [FromQuery(Name = "query")] string query = null
        )
        {
            bool queryIsPresentButCategoriesAreNot =
                (categories == null || categories.Count() == 0) && query != null;
            bool queryIsNotPresentButCategoriesAre =
                query == null && (categories != null && categories.Count() > 0);
            bool categoriesAndQueryAreNotPresent =
                (categories == null || categories.Count() == 0)
                && (query == null || string.IsNullOrWhiteSpace(query));

            if (queryIsPresentButCategoriesAreNot)
            {
                return Store.Instance.ProductsByQuery(query);
            }
            if (queryIsNotPresentButCategoriesAre)
            {
                return Store.Instance.ProductsByCategory(categories);
            }
            if (categoriesAndQueryAreNotPresent)
            {
                return Store.Instance.ProductsInStore;
            }
            else
            {
                return Store.Instance.ProductsByCategoryAndQuery(categories, query);
            }
        }
    }
}
