using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetCategoryByNameQuery :  IRequest<Category>
    {
        public string Name {get; set;}
    }
}