using System;

namespace TodoApi.Models
{
    public abstract class PaymentMethod
    {
        public enum Type
        {
            CASH = 0,
            SINPE = 1
        }

        public Type PaymentType { get; set; }

        public PaymentMethod(PaymentMethod.Type paymentType)
        {
            PaymentType = paymentType;
        }

        public static PaymentMethod Create(PaymentMethod.Type type)
        {
            switch (type)
            {
                case Type.CASH:
                    return new Cash();
                case Type.SINPE:
                    return new Sinpe();
                default:
                    throw new ArgumentException("Invalid payment type");
            }
        }
    }

    public sealed class Sinpe : PaymentMethod
    {
        public Sinpe() : base(PaymentMethod.Type.SINPE)
        {

        }
    }

    public sealed class Cash : PaymentMethod
    {
        public Cash() : base(PaymentMethod.Type.CASH)
        {

        }
    }
}