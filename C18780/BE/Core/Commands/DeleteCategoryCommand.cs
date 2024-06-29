using MediatR;
namespace StoreApi.Commands
{
    public sealed class DeleteCategoryCommand : IRequest<int>
    {
        public Guid Uuid { get; set; }
    }
}