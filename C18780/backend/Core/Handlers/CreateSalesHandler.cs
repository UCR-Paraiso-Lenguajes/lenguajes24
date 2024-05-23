using MediatR;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class CreateSalesHandler : IRequestHandler<CreateSalesCommand, Sales>
    {
        private readonly ISalesRepository _salesRepository;

        public CreateSalesHandler(ISalesRepository salesRepository)
        {
            if (salesRepository == null)
            {
                throw new ArgumentException("Illegal action, salesRepository is invalid.");
            }
            _salesRepository = salesRepository;
        }

        public async Task<Sales> Handle(CreateSalesCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var sales = new Sales()
            {
                Date = command.Date,
                Confirmation = command.Confirmation,
                PaymentMethod = command.PaymentMethods,
                Total = command.Total,
                Address = command.Address,
                PurchaseNumber = command.PurchaseNumber
            };

            return await _salesRepository.AddSalesAsync(sales);
        }
        private void ValidateCommand(CreateSalesCommand command)
        {
            if (command.Date == DateTime.MinValue)
            {
                throw new ArgumentException("The Date cannot be empty.");
            }
            if (command.Confirmation != 0 && command.Confirmation != 1)
            {
                throw new ArgumentException("Confirmation error.");
            }
            if (string.IsNullOrWhiteSpace(command.PaymentMethods))
            {
                throw new ArgumentException("The Paymet Methods cannot be empty.");
            }
            if (command.Total < 0)
            {
                throw new ArgumentException("The total must be greater than zero.");
            }
            if (string.IsNullOrWhiteSpace(command.Address))
            {
                throw new ArgumentException("The address cannot be empty.");
            }
        }
    }
}