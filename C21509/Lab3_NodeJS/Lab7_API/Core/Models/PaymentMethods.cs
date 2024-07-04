namespace Store_API.Models
{
    [Serializable]
    public abstract class PaymentMethods
    {
        public enum Type
        {
            CASH = 0,
            SINPE = 1
        }

        public Type PaymentType { get; set; }
        public bool IsEnabled { get; set; }

        public PaymentMethods(Type paymentType, bool isEnabled)
        {
            PaymentType = paymentType;
            IsEnabled = isEnabled;
        }

        private static Sinpe sinpe = new Sinpe();
        private static Cash cash = new Cash();

        public static PaymentMethods Find(Type type)
        {
            switch (type)
            {
                case Type.CASH:
                    return cash;
                case Type.SINPE:
                    return sinpe;
                default:
                    throw new NotImplementedException("Invalid payment method type.");
            }
        }

        public static PaymentMethods SetPaymentType(Type type)
        {
            return Find(type);
        }
    }

    [Serializable]
    public sealed class Sinpe : PaymentMethods
    {
        public Sinpe() : base(PaymentMethods.Type.SINPE, true)
        {
        }
    }

    [Serializable]
    public sealed class Cash : PaymentMethods
    {
        public Cash() : base(PaymentMethods.Type.CASH, true)
        {
        }
    }
}