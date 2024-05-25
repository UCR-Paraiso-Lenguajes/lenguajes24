using System;
using System.Collections.Generic;

namespace storeapi
{
    public sealed class Payment
    {
        public PaymentMethods Cash { get; private set; }
        public PaymentMethods Sinpe { get; private set; }

        public static readonly Payment Instance = new Payment();

        private Payment()
        {
            (Cash, Sinpe) = LoadPaymentMethodsFromDatabase();
        }

        private (PaymentMethods, PaymentMethods) LoadPaymentMethodsFromDatabase()
        {
            PaymentMethods cash = null;
            PaymentMethods sinpe = null;

            List<string[]> methodData = PaymentDB.RetrievePaymentMethods();

            foreach (string[] row in methodData)
            {
                if (row.Length >= 2)
                {
                    if (int.TryParse(row[0], out int id) && Enum.TryParse(row[1], out PaymentMethods.Type type))
                    {
                        if (type == PaymentMethods.Type.CASH)
                        {
                            cash = new Cash();
                        }
                        else if (type == PaymentMethods.Type.SINPE)
                        {
                            sinpe = new Sinpe();
                        }
                    }
                }
            }

            if (cash == null || sinpe == null)
            {
                throw new InvalidOperationException("No se encontraron ambos métodos de pago necesarios.");
            }

            return (cash, sinpe);
        }
    }
}
