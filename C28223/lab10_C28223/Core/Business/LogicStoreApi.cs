using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using storeApi.DataBase;
using storeApi.Models;

namespace storeApi.Business
{
    public sealed class LogicStoreApi
    {
        private readonly SaleDataBase saleDataBase = new SaleDataBase();

        public LogicStoreApi() { } // Se utiliza en la creacion del purchase

        public async Task<Sale> PurchaseAsync(Cart cart)
        {
            var productIdsIsEmpty = cart == null || cart.ProductIds == null || cart.ProductIds.Count == 0;
            var addressIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(cart.Address);
            if (productIdsIsEmpty) throw new ArgumentException($"Variable {nameof(cart)}must contain at least one product.");
            if (addressIsNullOrWhiteSpace) throw new ArgumentException("Address must be provided.");
            var store = await Store.Instance;
            var products = store.Products;
            var taxPercentage = store.TaxPercentage;
            // Obtener los productos que coinciden con los IDs del carrito
            IEnumerable<Product> matchingProducts = products
                .Where(p => cart.ProductIds.Any(pq => pq.ProductId == p.id.ToString())).ToList();
            // Crear una copia de los productos para evitar modificar los originales
            // Aqui cambia porque tiene que ahora  calcular la cantidad de productos y el precio de cada uno
            IEnumerable<Product> shadowCopyProducts = matchingProducts
                .Select(p =>
                {
                    var productQuantity = cart.ProductIds.FirstOrDefault(pq => pq.ProductId == p.id.ToString());
                    var clonedProduct = (Product)p.Clone();
                    clonedProduct.cant = productQuantity?.Quantity ?? 0; // Asignar la cantidad correspondiente
                    return clonedProduct;
                }).ToList();

            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                // Calcular el precio total incluyendo impuestos
                product.price *= (1 + taxPercentage / 100);
                purchaseAmount += product.price * product.cant;
            }
            // Obtener el método de pago seleccionado
            PaymentMethods selectedPaymentMethod = PaymentMethods.SetPaymentType(cart.PaymentMethod);
            // Crear un objeto de venta
            var sale = new Sale(GenerateNextPurchaseNumber(), shadowCopyProducts, cart.Address, purchaseAmount, selectedPaymentMethod.PaymentType);

            // Guardar la venta de forma asíncrona
            await saleDataBase.SaveAsync(sale);

            return sale;
        }

        private string GenerateNextPurchaseNumber()
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            string purchaseNumber = "";
            for (int i = 0; i < 6; i++)
            {
                purchaseNumber += chars[random.Next(chars.Length)];
            }
            return purchaseNumber;
        }

    }
}
