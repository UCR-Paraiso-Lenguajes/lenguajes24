using MediatR;

namespace StoreApi.Commands
{
    public class UpdateSalesCommand : IRequest<int>
    {
        public Guid Uuid { get; set; }
        public DateTime Date { get; set; }
        public int Confirmation { get; set; }
        public string PaymentMethods { get; set; }
        public decimal Total { get; set; }
        public string Address { get; set; }

        public UpdateSalesCommand(Guid uuid, DateTime date, int confirmation, string paymentMethods, decimal total, string address)
        {
            Uuid = uuid;
            Date = date;
            Confirmation = confirmation;
            PaymentMethods = paymentMethods;
            Total = total;
            Address = address;
        }
    }
}