using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Models;

namespace core.Business
{
    public sealed class StoreLogic
    {
        public StoreLogic() { }

        private CartDb data= new CartDb();

        public async Task<Sale> PurchaseAsync(Cart cart)
        {
            if (cart.ProductIds.Count == 0)
                throw new ArgumentException("Carrito debe tener al menos un producto");

            if (string.IsNullOrWhiteSpace(cart.Address))
                throw new ArgumentException("Se debe asignar una direccion");

            var products = Store.Instance.Products;
            var taxPercentage = Store.Instance.TaxPercentage;

            IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.id.ToString())).ToList();
            List<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            decimal purchaseAmount = 0;

            foreach (var product in shadowCopyProducts)
            {
                product.price *= (1 + (decimal)taxPercentage / 100);
                purchaseAmount += product.price;
            }

            PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

            PaymentMethods selectedPaymentMethod = PaymentMethods.SetPaymentType(cart.PaymentMethod);

            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, selectedPaymentMethod.PaymentType, Sale.generarNumeroCompra()); 

            data.saveAsync(sale); 

            return sale;
        }
    }
}