using System;
using System.Collections.Generic;

namespace storeapi.Models
{
    public sealed class Cart
    {
        private List<string> _productIds;
        private string _address;
        private PaymentMethods.Type _paymentMethod;

        public List<string> ProductIds
        {
            get => _productIds;
            set
            {
                if (value == null || value.Count == 0)
                {
                    throw new ArgumentException("La lista de identificadores de productos no puede ser nula o vacía.");
                }
                _productIds = value;
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La dirección no puede ser nula o vacía.");
                }
                _address = value;
            }
        }

        public PaymentMethods.Type PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                if (!Enum.IsDefined(typeof(PaymentMethods.Type), value))
                {
                    throw new ArgumentException("El método de pago especificado no es válido.");
                }
                _paymentMethod = value;
            }
        }
    }
}
