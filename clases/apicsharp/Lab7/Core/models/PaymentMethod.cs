
namespace TodoApi.models;


public abstract class PaymentMethods{
    public enum Type 
    {
    CASH=0,
    SINPE=1
    }
    public Type PaymentType { get; set; }
    public PaymentMethods(PaymentMethods.Type paymentType)
    {
        PaymentType= paymentType;

    }
    private static readonly Cash cash = new Cash();
    private static readonly Sinpe sinpe = new Sinpe();
    public static PaymentMethods Find(PaymentMethods.Type type)
    {
        switch (type)
        {
            case PaymentMethods.Type.CASH:
                return cash;
            case PaymentMethods.Type.SINPE:
                return sinpe;
            default: throw new NotImplementedException();
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