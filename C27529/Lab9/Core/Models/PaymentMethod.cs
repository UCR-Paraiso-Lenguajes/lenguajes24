namespace storeApi;

public abstract class PaymentMethod
{
    public enum Type
    {CASH = 0,SINPE = 1}
    public Type PaymentType { get; set; }
    public PaymentMethod(PaymentMethod.Type paymentType)
    {
        PaymentType = paymentType;

    }
    private static Sinpe sinpe=new Sinpe();
    private static Cash cash=new Cash();

    public static PaymentMethod Find(Type type)
    {

        switch (type)
        {
            case Type.CASH:
                return cash;
            case Type.SINPE:
                return sinpe;
            default:
                throw new ArgumentException("Invalid payment method type.");
        }
    }
     public static PaymentMethod SetPaymentType(Type type)
    {
        return Find(type);
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