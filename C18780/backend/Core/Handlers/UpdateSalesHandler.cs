using MediatR;
using StoreApi.Commands;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class UpdateSalesHandler : IRequestHandler<UpdateSalesCommand, int>
    {
        private readonly ISalesRepository _salesRepository;

        public UpdateSalesHandler(ISalesRepository salesRepository)
        {
            if (salesRepository == null)
            {
                throw new ArgumentException("Illegal action, salesRepository is invalid.");
            }
            _salesRepository = salesRepository;
        }
        public async Task<int> Handle(UpdateSalesCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);
            var sales = await _salesRepository.GetSalesByIdAsync(command.Uuid);
            if (sales == null)
                return default;

            sales.Date = command.Date;
            sales.Confirmation = command.Confirmation;
            sales.PaymentMethod = command.PaymentMethods;
            sales.Total = command.Total;
            sales.Address = command.Address;
            sales.PurchaseNumber = command.PurchaseNumber;


            return await _salesRepository.UpdateSalesAsync(sales);
        }
        private void ValidateCommand(UpdateSalesCommand command)
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