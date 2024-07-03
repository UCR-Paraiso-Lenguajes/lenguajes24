using MediatR;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class CreateSinpeHandler : IRequestHandler<CreateSinpeCommand, Sinpe>
    {
        private readonly ISinpeRepository _sinpeRepository;

        public CreateSinpeHandler(ISinpeRepository sinpeRepository)
        {
            if (sinpeRepository == null)
            {
                throw new ArgumentException("Illegal action, sinpeRepository is invalid.");
            }
            _sinpeRepository = sinpeRepository;
        }

        public async Task<Sinpe> Handle(CreateSinpeCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);
            var sinpe = new Sinpe()
            {
                SalesUuid = command.SalesUuid,
                ConfirmationNumber = command.ConfirmationNumber
            };

            return await _sinpeRepository.AddSinpeAsync(sinpe);
        }

        private void ValidateCommand(CreateSinpeCommand command)
        {
            if (command.SalesUuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(command.ConfirmationNumber))
            {
                throw new ArgumentException("The confirmation number cannot be empty.");
            }
        }
    }
}