using MediatR;
using StoreApi.Models;

namespace StoreApi.Commands
{
    public sealed class CreateSinpeCommand : IRequest<Sinpe>
    {
        public Guid SalesUuid {get; set; }
        public string ConfirmationNumber {get; set; }
        public CreateSinpeCommand(Guid salesUuid, string confirmationNumber)
        {
            SalesUuid = salesUuid;
            ConfirmationNumber = confirmationNumber;
        }
    }
}