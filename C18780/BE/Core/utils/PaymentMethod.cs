using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.utils
{
    [NotMapped]
    public abstract class PaymentMethods
    {
        public enum Type
        {
            CASH = 0,
            SINPE = 1
        }

        public Type PaymentType { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        protected PaymentMethods(Type paymentType, string name, bool isEnabled)
        {
            PaymentType = paymentType;
            Name = name;
            IsEnabled = isEnabled;
        }

        public static Type Find(Type type)
        {
            switch (type)
            {
                case Type.CASH:
                    return Type.CASH;
                case Type.SINPE:
                    return Type.SINPE;
                default:
                    throw new NotImplementedException("Not valid");
            }
        }
    }

    public sealed class SinpeMovil : PaymentMethods
    {
        public SinpeMovil() : base(Type.SINPE, "Sinpe Móvil", true)
        {
        }
    }

    public sealed class Cash : PaymentMethods
    {
        public Cash() : base(Type.CASH, "Cash", true)
        {
        }
    }
}
