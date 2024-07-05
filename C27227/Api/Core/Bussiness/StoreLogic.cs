using System;
using System.Text;
using System.Threading.Tasks;
using KEStoreApi.Models;
using KEStoreApi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KEStoreApi.Bussiness
{
    public sealed class StoreLogic
    {
        private static Random randomNumber = new Random();
        private DatabaseSale saleDataBase = new DatabaseSale();

        public StoreLogic() { }

        public async Task<Sale> PurchaseAsync(Cart cart)
        {
            if (cart == null) 
            { 
                throw new ArgumentNullException($"El objeto {nameof(cart)} no puede ser nulo."); 
            }
            if (cart.Product == null || cart.Product.Count == 0) 
            { 
                throw new ArgumentException($"El {nameof(cart.Product.Count)} debe contener al menos un producto."); 
            }
            if (!IsValidAddress(cart.Address)) 
            { 
                throw new ArgumentException($"Se debe proporcionar una dirección válida"); 
            }

            var storeInstance = await Store.Instance;
            var products = storeInstance.ProductsList;
            var taxPercentage = storeInstance.TaxPercentage;

            List<Product> matchingProducts = new List<Product>();
            decimal subtotal = 0;

            foreach (var productQuantity in cart.Product)
            {
                int productId = productQuantity.Id;
                int quantity = productQuantity.Quantity;

                Product matchingProduct = products.FirstOrDefault(p => p.Id == productId);
                if (matchingProduct == null) 
                { 
                    throw new ArgumentException($"El {nameof(cart.Product)} con ID {productId} no existe en la tienda."); 
                }

                if (quantity <= 0) 
                { 
                    throw new ArgumentException($"La cantidad del {nameof(cart.Product)} con ID {productId} debe ser mayor que cero."); 
                }

                Product copiedProduct = (Product)matchingProduct.Clone();
                copiedProduct.Quantity = quantity;

                matchingProducts.Add(copiedProduct);
                decimal totalPrice = quantity * matchingProduct.Price;
                subtotal += totalPrice;
            }

            decimal taxAmount = subtotal * ((decimal)taxPercentage / 100);
            decimal total = subtotal + taxAmount;

            PaymentMethods paymentMethod = PaymentMethods.SetPaymentType((PaymentMethods.Type)cart.PaymentMethod);

            if (!IsPaymentMethodEnabled(paymentMethod, storeInstance))
            {
                throw new InvalidOperationException($"El método de pago {paymentMethod.PaymentType} está deshabilitado.");
            }

            var purchaseNumberSale = GeneratePurchaseNumber();
            var sale = new Sale(purchaseNumberSale, matchingProducts, cart.Address, total, paymentMethod.PaymentType);
            await saleDataBase.SaveAsync(sale);
            return sale;
        }

        public static bool IsValidAddress(Address address)
        {
            var zipCodePattern = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");

            return !string.IsNullOrEmpty(address.Street) && address.Street.Trim().Length >= 5 &&
                   !string.IsNullOrEmpty(address.City) && address.City.Trim().Length >= 2 &&
                   !string.IsNullOrEmpty(address.State) && address.State.Trim().Length >= 2 &&
                   !string.IsNullOrEmpty(address.ZipCode) && zipCodePattern.IsMatch(address.ZipCode) &&
                   !string.IsNullOrEmpty(address.Country) && address.Country.Trim().Length >= 2;
        }

        private bool IsPaymentMethodEnabled(PaymentMethods paymentMethod, Store storeInstance)
        {
            var method = storeInstance.PaymentMethodsList.FirstOrDefault(m => m.PaymentType == paymentMethod.PaymentType);
            return method != null && method.IsEnabled;
        }

        private string GeneratePurchaseNumber()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int charType = randomNumber.Next(0, 3);
                switch (charType)
                {
                    case 0:
                        sb.Append(randomNumber.Next(0, 10));
                        break;
                    case 1:
                        sb.Append((char)randomNumber.Next('A', 'Z' + 1));
                        break;
                    case 2:
                        sb.Append((char)randomNumber.Next('a', 'z' + 1));
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
