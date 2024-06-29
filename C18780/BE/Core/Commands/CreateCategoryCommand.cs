using MediatR;
using StoreApi.Models;
namespace StoreApi.Commands
{
    public sealed class CreateCategoryCommand : IRequest<Category>
    {
        public string Name { get; set; }
        public CreateCategoryCommand(string name)
        {
            Name = name;
        }
    }
}