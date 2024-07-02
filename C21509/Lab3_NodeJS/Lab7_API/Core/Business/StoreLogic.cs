using Store_API.Database;
using Store_API.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store_API.Business
{
    public sealed class StoreLogic
    {
        public DB_API dbAPI = new DB_API();

        public async Task<string> PurchaseAsync(Cart cart)
        {
            var products = Store.Instance.Products;
            var taxPercentage = Store.Instance.TaxPercentage;

            // Encontrar productos basados en los IDs en el carrito
            var matchingProducts = products.Where(p => cart.Products.Any(cp => cp.Id == p.Id)).ToList();

            decimal purchaseAmount = 0;
            foreach (var productQuantity in cart.Products)
            {
                var product = matchingProducts.FirstOrDefault(p => p.Id == productQuantity.Id);
                if (product != null)
                {
                    purchaseAmount += product.Price * productQuantity.Quantity;
                }
            }

            PaymentMethods.Type paymentMethodType = cart.PaymentMethod;

            string purchaseNumber = GeneratePurchaseNumber();

            // Crear una lista de ProductQuantity con el precio incluido
            var productQuantities = cart.Products.Select(pq =>
            {
                var product = matchingProducts.First(mp => mp.Id == pq.Id);
                return new ProductQuantity(pq.Id, pq.Quantity, product.Price);
            }).ToList();

            var sale = new Sale(matchingProducts, cart.Address, purchaseAmount, paymentMethodType, purchaseNumber);

            sale.PurchaseNumber = purchaseNumber;
            await dbAPI.InsertSaleAsync(sale, productQuantities);

            return purchaseNumber;
        }

        private string GeneratePurchaseNumber()
        {
            Random random = new Random();
            string letters = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string numbers = random.Next(100, 999).ToString();

            return $"{letters}{numbers}";
        }
    }
}