using MediatR;
using StoreApi.Models;

namespace StoreApi.Commands
{
    public sealed class CreateAdCommand : IRequest<Ad>
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public CreateAdCommand(string message, DateTime date)
        {
            Message = message;
            Date = date;
        }
    }
}