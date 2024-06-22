using MediatR;

namespace StoreApi.Commands
{
    public sealed class DeleteAdCommand : IRequest<int>
    {
        public Guid Uuid { get; set; }
    }
}