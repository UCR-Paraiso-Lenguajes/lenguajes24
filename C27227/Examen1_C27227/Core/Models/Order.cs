using KEStoreApi;
using KEStoreApi.Models;

namespace Core;

public class Order
{
        public PaymentMethods.Type PaymentMethod { get; set; }
         public Address Address { get; set; }
}
