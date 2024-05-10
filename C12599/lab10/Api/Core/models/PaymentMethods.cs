using System;

namespace storeapi.Models
{
    public abstract class PaymentMethods
    {
        public enum Type
        {
            CASH = 0,
            SINPE = 1
        }

        public Type PaymentType { get; set; }

        protected PaymentMethods(Type paymentType)
        {
            PaymentType = paymentType;
        }

        private static readonly Sinpe sinpe = new Sinpe();
        private static readonly Cash cash = new Cash();

        public static PaymentMethods Find(Type type)
        {
            switch (type)
            {
                case Type.CASH:
                    return cash;
                case Type.SINPE:
                    return sinpe;
                default:
                    throw new NotImplementedException("Payment method not implemented");
            }
        }

        public static PaymentMethods SetPaymentType(Type type)
        {
            return Find(type);
        }
    }

    public sealed class Sinpe : PaymentMethods
    {
        public Sinpe() : base(Type.SINPE)
        {

        }
    }

    public sealed class Cash : PaymentMethods
    {
        public Cash() : base(Type.CASH)
        {

        }
    }
}
