namespace ApiLab7;
public abstract class PaymentMethods{
    public enum Type 
    {
        CASH=0,
        SINPE=1
    }
    public Type PaymentType { get; set; }
    public bool IsEnabled { get; set; }

    public PaymentMethods(Type paymentType, bool isEnabled = true)
    {
        PaymentType = paymentType;
        IsEnabled = isEnabled;
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

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
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