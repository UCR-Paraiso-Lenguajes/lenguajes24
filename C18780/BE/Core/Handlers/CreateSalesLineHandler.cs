using MediatR;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class CreateSalesLineHandler : IRequestHandler<CreateSalesLineCommand, SalesLine>
    {
        private readonly ISalesLineRepository _salesLineRepository;

        public CreateSalesLineHandler(ISalesLineRepository salesLineRepository)
        {
            if (salesLineRepository == null)
            {
                throw new ArgumentException("Illegal action, salesLineRepository is invalid.");
            }
            _salesLineRepository = salesLineRepository;
        }

        public async Task<SalesLine> Handle(CreateSalesLineCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var salesLine = new SalesLine()
            {
                Quantity = command.Quantity,
                Subtotal = command.Subtotal,
                UuidProduct = command.UuidProduct,
                UuidSales = command.UuidSales
            };

            return await _salesLineRepository.AddSalesLineAsync(salesLine);
        }

        private void ValidateCommand(CreateSalesLineCommand command)
        {
            if (command.Quantity < 0)
            {
                throw new ArgumentException("The quantity must be greater than zero.");
            }
            if (command.Subtotal < 0)
            {
                throw new ArgumentException("The subtotal must be greater than zero.");
            }
            if (command.UuidProduct == Guid.Empty)
            {
                throw new ArgumentException("The uuid product cannot be empty.");
            }
            if (command.UuidSales == Guid.Empty)
            {
                throw new ArgumentException("The uuid sales cannot be empty.");
            }
        }
    }
}