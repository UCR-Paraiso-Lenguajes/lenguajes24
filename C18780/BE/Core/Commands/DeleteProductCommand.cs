using MediatR;

namespace StoreApi.Commands
{
    public sealed class DeleteProductCommand : IRequest<int>
    {
        public Guid Uuid { get; set; }
    }
}