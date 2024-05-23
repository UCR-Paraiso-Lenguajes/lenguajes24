using MediatR;
using StoreApi.Models;

namespace StoreApi.Commands
{
    public sealed class CreateSinpeCommand : IRequest<Sinpe>
    {
        public Guid UuidSales {get; set; }
        public string ConfirmationNumber {get; set; }
        public CreateSinpeCommand(Guid uuidSales, string confirmationNumber)
        {
            UuidSales = uuidSales;
            ConfirmationNumber = confirmationNumber;
        }
    }
}