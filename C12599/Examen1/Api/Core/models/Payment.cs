using System;
using System.Collections.Generic;
using storeapi.Database;

namespace storeapi.Models
{
    public sealed class Payment
    {
        public PaymentMethods Cash { get; private set; }
        public PaymentMethods Sinpe { get; private set; }

        public static readonly Payment Instance = new Payment();

        private Payment()
        {
            var methods = LoadPaymentMethodsFromDatabase();
            Cash = methods[PaymentMethods.Type.CASH];
            Sinpe = methods[PaymentMethods.Type.SINPE];

            if (Cash == null || Sinpe == null)
            {
                throw new InvalidOperationException("No se encontraron ambos métodos de pago necesarios.");
            }
        }

        private Dictionary<PaymentMethods.Type, PaymentMethods> LoadPaymentMethodsFromDatabase()
        {
            var paymentMethods = new Dictionary<PaymentMethods.Type, PaymentMethods>();
            var data = PaymentDB.RetrievePaymentMethods();

            foreach (var methodData in data)
            {
                int id = int.Parse(methodData[0]);
                string name = methodData[1];
                bool isActive = bool.Parse(methodData[2]);

                var method = PaymentMethods.LoadFromDatabase(id, name, isActive);
                paymentMethods.Add((PaymentMethods.Type)id, method);
            }

            return paymentMethods;
        }
    }
}

