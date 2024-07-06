public abstract class PaymentMethod
{
    public enum Type { CASH = 0, SINPE = 1 }
    public Type PaymentType { get; set; }
    public bool Enabled { get; set; }

    public PaymentMethod() { }

    public PaymentMethod(PaymentMethod.Type paymentType)
    {
        PaymentType = paymentType;
    }

    public static PaymentMethod Find(Type type)
    {
        switch (type)
        {
            case Type.CASH:
                return new Cash();
            case Type.SINPE:
                return new Sinpe();
            default:
                throw new NotImplementedException("Invalid payment method type.");
        }
    }
}

public sealed class Sinpe : PaymentMethod
{
    public Sinpe() : base(PaymentMethod.Type.SINPE)
    {
        Enabled = true;
    }
}

public sealed class Cash : PaymentMethod
{
    public Cash() : base(PaymentMethod.Type.CASH)
    {
        Enabled = true;
    }
}
