using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetAdByIdQuery : IRequest<Ad>
    {
        public Guid Uuid { get; set; }
    }
}