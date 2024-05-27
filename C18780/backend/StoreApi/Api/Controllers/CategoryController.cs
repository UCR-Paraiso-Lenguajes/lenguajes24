using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Cache;
using StoreApi.Queries;
using Microsoft.AspNetCore.Authorization;
namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;
        private List<Categories> categories;
        public CategoryController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            this.mediator = mediator;
            categories = new List<Categories>();

            LoadCategoriesFromDatabaseAsync().Wait();
        }

        private async Task LoadCategoriesFromDatabaseAsync()
        {
            if (CategoriesCache._categories == null || !CategoriesCache._categories.Any())
            {
                var result = await mediator.Send(new GetCategoryListQuery());
                if (result != null)
                {
                    foreach (var category in result)
                    {
                        categories.Add(new Categories(category));
                    }
                    categories = categories.OrderBy(x => x.Name).ToList<Categories>();
                    CategoriesCache._categories = categories;
                }
            }
            else
            {
                categories = (List<Categories>)CategoriesCache._categories;
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Categories>> GetCategoryListAsync()
        {
            return categories;
        }

        [HttpGet("categoryId")]
        [AllowAnonymous]
        public Categories GetCategoryById(Guid uuid)
        {
            foreach (var category in categories)
            {
                if (category.Uuid == uuid)
                {
                    return category;
                }
            }
            return default;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<Category> AddCategoryAsync(Category newCategory)
        {
            var category = await mediator.Send(new CreateCategoryCommand(
                    newCategory.Name
                ));

            return category;
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<int> DeleteCategoryAsync(Guid Uuid)
        {
            return await mediator.Send(new DeleteCategoryCommand() { Uuid = Uuid });
        }
    }
}