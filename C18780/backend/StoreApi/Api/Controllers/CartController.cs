using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.utils;

namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<PurchaseNumber> AddCartAsync([FromBody] Cart cart)
        {
            PaymentMethods.Type paymentMethod = (PaymentMethods.Type)cart.PaymentMethod;
            string paymentMethodName = Enum.GetName(typeof(PaymentMethods.Type), paymentMethod);
            decimal total = 0;

            var createSalesCommand = new CreateSalesCommand(
                date: DateTime.Now,
                confirmation: 0,
                paymentMethods: paymentMethodName,
                total: total,
                address: cart.Address,
                purchaseNumber: Utils.GetPurchaseNumber()
            );

            var sales = await _mediator.Send(createSalesCommand);

            foreach (var productId in cart.ProductIds)
            {
                var product = await GetProductById(Guid.Parse(productId));

                if (product != null)
                {
                    total += product.Price;

                    var createSalesLineCommand = new CreateSalesLineCommand(
                        quantity: 1,
                        subTotal: product.Price,
                        uuidProduct: Guid.Parse(productId),
                        uuidSales: sales.Uuid
                    );

                    await _mediator.Send(createSalesLineCommand);
                }
            }

            sales.Total = total;

            var updateSalesCommand = new UpdateSalesCommand(
                sales.Uuid,
                sales.Date,
                sales.Confirmation,
                sales.PaymentMethod,
                sales.Total,
                sales.Address,
                sales.PurchaseNumber
            );

            await _mediator.Send(updateSalesCommand);
            return new PurchaseNumber(sales.PurchaseNumber);
        }

        private async Task<Product> GetProductById(Guid productId)
        {
            var productController = new ProductController(_mediator);

            return await productController.GetProductByIdAsync(productId);
        }
    }
}