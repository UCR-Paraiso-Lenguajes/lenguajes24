using System;
using System.Collections.Generic;
using System.Linq;
using StoreAPI.models;
using StoreAPI.Database;

namespace StoreAPI.Business
{
    public sealed class StoreLogic
    {
        private readonly SaleBD saleDB = new SaleBD();
        private readonly PaymentMethodDB paymentMethodDB;

        public StoreLogic(PaymentMethodDB paymentMethodDB)
        {
            this.paymentMethodDB = paymentMethodDB;
        }

        public async Task<Sale> PurchaseAsync(Cart cart)
        {
            var productIdsIsEmpty = cart == null || cart.ProductIds == null || cart.ProductIds.Count() == 0;
            var addressIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(cart.Address);
            if (productIdsIsEmpty) throw new ArgumentException($"Variable {nameof(cart)}must contain at least one product.");
            if (addressIsNullOrWhiteSpace) throw new ArgumentException("Address must be provided.");
            if (cart == null || cart.ProductIds == null) throw new ArgumentException("The cart cannot be empty.");
            if (!IsValidAddress(cart.Address)) throw new ArgumentException("La dirección proporcionada no es válida.");
            bool isPaymentMethodActive = await paymentMethodDB.IsPaymentMethodActiveAsync(cart.PaymentMethod.Id);
            if (!isPaymentMethodActive)
            {
                throw new ArgumentException("El método de pago seleccionado no está activo.");
            }



            var storeInstance = await Store.Instance.Value;
            var products = storeInstance.Products;
            var taxPercentage = storeInstance.TaxPercentage;

            // Find matching products based on the product IDs in the cart
            IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Any(pq => pq.ProductId == p.Id.ToString())).ToList();

            // Create shadow copies of the matching products
            IEnumerable<Product> shadowCopyProductss = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            IEnumerable<Product> shadowCopyProducts = matchingProducts
                          .Select(p =>
                          {
                              var productQuantity = cart.ProductIds.FirstOrDefault(pq => pq.ProductId == p.Id.ToString());
                              var clonedProduct = (Product)p.Clone();
                              clonedProduct.Quantity = productQuantity?.Quantity ?? 0; // Asignar la cantidad correspondiente
                              return clonedProduct;
                          }).ToList();
            // Calculate purchase amount by multiplying each product's price with the store's tax percentage
            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                product.Price *= (1 + taxPercentage / 100);
                purchaseAmount += product.Price * product.Quantity;
            }

            string purchaseNumber = GenerateNextPurchaseNumber();

            PaymentMethods.Type paymentMethodType = cart.PaymentMethod.Id == 1 ? PaymentMethods.Type.CASH : PaymentMethods.Type.SINPE;

            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethodType, purchaseNumber);

            await saleDB.SaveAsync(sale);

            return sale;
        }

        private string GenerateNextPurchaseNumber()
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomLetters = new string(Enumerable.Repeat(chars, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int randomNumber = random.Next(100000, 999999);

            string purchaseNumber = $"{randomLetters}-{randomNumber}";

            return purchaseNumber;
        }

        private bool IsValidAddress(string address)
        {

            var provinces = new Dictionary<string, List<string>>
            {
                { "San José", new List<string> { "Central", "Escazú", "Desamparados", "Puriscal", "Tarrazú", "Aserrí", "Mora", "Goicoechea", "Santa Ana", "Alajuelita", "Vásquez de Coronado", "Acosta", "Tibás", "Moravia", "Montes de Oca", "Turrubares", "Dota", "Curridabat", "Pérez Zeledón", "León Cortés", "San Pedro" } },
                { "Alajuela", new List<string> { "Central", "San Ramón", "Grecia", "San Mateo", "Atenas", "Naranjo", "Palmares", "Poás", "Orotina", "San Carlos", "Zarcero", "Valverde Vega", "Upala", "Los Chiles", "Guatuso", "Río Cuarto" } },
                { "Cartago", new List<string> { "Central", "Paraíso", "La Unión", "Jiménez", "Turrialba", "Alvarado", "Oreamuno", "El Guarco" } },
                { "Heredia", new List<string> { "Central", "Barva", "Santo Domingo", "Santa Bárbara", "San Rafael", "San Isidro", "Belén", "Flores", "San Pablo", "Sarapiquí" } },
                { "Guanacaste", new List<string> { "Liberia", "Nicoya", "Santa Cruz", "Bagaces", "Carrillo", "Cañas", "Abangares", "Tilarán", "Nandayure", "La Cruz", "Hojancha" } },
                { "Puntarenas", new List<string> { "Central", "Esparza", "Buenos Aires", "Montes de Oro", "Osa", "Quepos", "Golfito", "Coto Brus", "Parrita", "Corredores", "Garabito" } },
                { "Limón", new List<string> { "Central", "Pococí", "Siquirres", "Talamanca", "Matina", "Guácimo" } }
            };


            var parts = address.Split(',');
            if (parts.Length < 2) return false;

            var province = parts[0].Trim();
            var canton = parts[1].Trim();

            return provinces.ContainsKey(province) && provinces[province].Contains(canton);
        }

    }
}