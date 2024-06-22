using MediatR;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApi.Handler
{
    public sealed class CreateAdHandler : IRequestHandler<CreateAdCommand, Ad>
    {
        private readonly IAdRepository _adRepository;

        public CreateAdHandler(IAdRepository adRepository)
        {
            if (adRepository == null)
            {
                throw new ArgumentException("Illegal action, adRepository is invalid.");
            }
            _adRepository = adRepository;
        }

        public async Task<Ad> Handle(CreateAdCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var ad = new Ad()
            {
                Date = command.Date,
                Message = command.Message
            };

            return await _adRepository.AddAdAsync(ad);
        }

        private void ValidateCommand(CreateAdCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Message))
            {
                throw new ArgumentException("The message cannot be empty.");
            }
        }
    }
}