using MediatR;
using StoreApi.Models;
using StoreApi.utils;

namespace StoreApi.Commands
{
    public sealed class CreateSalesCommand : IRequest<Sales>
    {
        public DateTime Date { get; set; }
        public int Confirmation {get; set;}
        public string PaymentMethods {get; set;}
        public decimal Total {get; set;}
        public string Address {get; set;}
        public string PurchaseNumber {get; set;}
        public CreateSalesCommand(DateTime date, int confirmation, string paymentMethods, decimal total, string address, string purchaseNumber)
        {
            Date = date;
            Confirmation = confirmation;
            PaymentMethods = paymentMethods;
            Total = total;
            Address = address;
            PurchaseNumber = purchaseNumber;
        }
    }
}