using MediatR;
using StoreApi.Models;

namespace StoreApi.Commands
{
    public sealed class CreateSalesLineCommand : IRequest<SalesLine>
    {
        public int Quantity {get; set; }
        public decimal Subtotal {get; set;}
        public Guid UuidProduct {get; set;}
        public Guid UuidSales {get; set;}
        public CreateSalesLineCommand(int quantity, decimal subTotal, Guid uuidProduct, Guid uuidSales)
        {
            Quantity = quantity;
            Subtotal = subTotal;
            UuidProduct = uuidProduct;
            UuidSales = uuidSales;
        }
    }
}