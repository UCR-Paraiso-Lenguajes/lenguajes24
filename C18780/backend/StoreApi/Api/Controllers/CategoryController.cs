using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Queries;

namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly List<Categories> categories;
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
            var result = await mediator.Send(new GetCategoryListQuery());
            if (result != null)
            {
                foreach (var category in result)
                {
                    categories.Add(new Categories(category));
                }
            }
            categories.Sort(new CategoryNameComparator());
        }

        [HttpGet]
        public async Task<List<Categories>> GetCategoryListAsync()
        {
            return categories;
        }

        [HttpGet("categoryId")]
        public Category GetCategoryById(Guid uuid)
        {
            return default;
        }
        [HttpGet("categoryName")]
        public Category GetCategoryByName(string name)
        {
            return default;
        }

        [HttpPost]
        public async Task<Category> AddCategoryAsync(Category newCategory)
        {
            var category = await mediator.Send(new CreateCategoryCommand(
                    newCategory.Name
                ));

            return category;
        }

        [HttpDelete]
        public async Task<int> DeleteCategoryAsync(Guid Uuid)
        {
            return await mediator.Send(new DeleteCategoryCommand() { Uuid = Uuid });
        }
    }
}