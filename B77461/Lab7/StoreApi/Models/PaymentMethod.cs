namespace StoreApi.Models
{
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
                return new Cash();
            case PaymentMethods.Type.SINPE:
                return new Sinpe();
            default: return null;
        }
    }
}
public sealed class Sinpe:PaymentMethods{
    public Sinpe(): base(PaymentMethods.Type.SINPE)
    {

    }
}
public sealed class Cash:PaymentMethods{
    public Cash(): base(PaymentMethods.Type.CASH)
    {

    }
}
}