using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRWebpack.Hubs;
using StoreApi.Cache;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Search;
using StoreApi.utils;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class StoreController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly CategoryController categoryController;
        private CategoriesCache categoriesCache;
        private ProductsCache productsCache = ProductsCache.GetInstance();
        private readonly IHubContext<PaymentMethodsHub> _hubContext;

        public StoreController(IMediator mediator, CategoryController categoryController, IHubContext<PaymentMethodsHub> hubContext)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));

            this.mediator = mediator;
            this.categoryController = categoryController;
            categoriesCache = CategoriesCache.GetInstance();
        }
        [HttpGet("Products")]
        [AllowAnonymous]
        public async Task<Store> GetStoreAsync([FromQuery] List<string> category, string search)
        {
            List<Product> products = new List<Product>();
            //Paso 1: Guardo los productos en memoria para no volver a pedirlos a base de datos
            if (!productsCache.exists())
            {
                var productsList = await mediator.Send(new GetProductListQuery());
                productsCache.setProduct(productsList);
            }
            //Paso 2: Filtro los productos por su categoria
            if (!category.Contains("All"))
            {
                foreach (var c in category)
                {
                    products.AddRange(productsCache.GetProduct(categoriesCache.GetCategoryByName(c).Uuid));
                }
            }
            else
            {
                products.AddRange(productsCache.getAll());
            }
            //Paso 3: Filtro los productos por el search
            if (!search.Equals("none"))
            {
                ProductSearch productSearch = new ProductSearch(products);
                products = new List<Product>(productSearch.Search(search));
            }

            return new Store(products, paymentMethods);
        }

        private static List<PaymentMethods> paymentMethods = new List<PaymentMethods>
        {
            new SinpeMovil(),
            new Cash()
        };

        [HttpPost("UpdatePaymentEnabled"), Authorize(Roles = "Operator")]
        public async Task<IActionResult> UpdatePaymentEnabledAsync([FromBody] int paymentMethod)
        {
            PaymentMethods.Type paymentMethodType = (PaymentMethods.Type)paymentMethod;
            PaymentMethods payment = paymentMethods.Find(p => p.PaymentType == paymentMethodType);
            payment.IsEnabled = !payment.IsEnabled;
            await _hubContext.Clients.All.SendAsync("PaymentMethodsChange", paymentMethods);
            return Ok();
        }

        [HttpGet("PaymentMethods")]
        [AllowAnonymous]
        public Task<List<PaymentMethods>> GetPaymentMethods()
        {
            return Task.FromResult(paymentMethods);
        }
    }
}