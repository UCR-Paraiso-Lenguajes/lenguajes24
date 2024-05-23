using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetProductByIdQuery :  IRequest<Product>
    {
        public Guid Uuid { get; set; }
    }
}