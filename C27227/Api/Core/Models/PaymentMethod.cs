public abstract class PaymentMethods
{
    public enum Type
    {
        CASH = 0,
        SINPE = 1
    }

    public Type PaymentType { get; set; }
    public bool IsEnabled { get; set; }

    protected PaymentMethods(Type paymentType)
    {
        PaymentType = paymentType;
        IsEnabled = true; // Por defecto, todos los métodos están habilitados
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
                throw new NotImplementedException("El tipo de método de pago especificado no está implementado.");
        }
    }

    public static PaymentMethods SetPaymentType(Type type)
    {
        return Find(type);
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public sealed class Sinpe : PaymentMethods
    {
        public Sinpe() : base(Type.SINPE) { }
    }

    public sealed class Cash : PaymentMethods
    {
        public Cash() : base(Type.CASH) { }
    }
}
