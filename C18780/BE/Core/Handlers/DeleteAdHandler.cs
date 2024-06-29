using MediatR;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApi.Handler
{
    public sealed class DeleteAdHandler : IRequestHandler<DeleteAdCommand, int>
    {
        private readonly IAdRepository _adRepository;

        public DeleteAdHandler(IAdRepository adRepository)
        {
            if (adRepository == null)
            {
                throw new ArgumentException("Illegal action, adRepository is invalid.");
            }
            _adRepository = adRepository;
        }

        public async Task<int> Handle(DeleteAdCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var ad = await _adRepository.GetAdByIdAsync(command.Uuid);
            if (ad == null)
                return default;

            return await _adRepository.DeleteAdAsync(ad.Uuid);
        }

        private void ValidateCommand(DeleteAdCommand command)
        {
            if (command.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}