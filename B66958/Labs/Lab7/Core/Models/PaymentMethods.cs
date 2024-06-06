namespace ApiLab7;
public abstract class PaymentMethods{
    public enum Type 
    {
        CASH=0,
        SINPE=1
    }
    public Type PaymentType { get; set; }

    public PaymentMethods(PaymentMethods.Type paymentType)
    {
        PaymentType = paymentType;

    }

    public static PaymentMethods Find(PaymentMethods.Type type)
    {
        switch (type)
        {
            case PaymentMethods.Type.CASH:
                return Cash.Instance;
            case PaymentMethods.Type.SINPE:
                return Sinpe.Instance;
            default: return null;
        }
    }
}
public sealed class Sinpe : PaymentMethods
{
    private static readonly Sinpe instance = new Sinpe();

    public static Sinpe Instance => instance;

    private Sinpe() : base(PaymentMethods.Type.SINPE)
    {
    }
}

public sealed class Cash : PaymentMethods
{
    private static readonly Cash instance = new Cash();

    public static Cash Instance => instance;

    private Cash() : base(PaymentMethods.Type.CASH)
    {
    }
}