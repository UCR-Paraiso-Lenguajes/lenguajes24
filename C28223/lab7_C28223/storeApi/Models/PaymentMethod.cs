namespace storeApi;

public abstract class PaymentMethods
{
    public enum Type
    {CASH = 0,SINPE = 1}
    public Type PaymentType { get; set; }
    public PaymentMethods(PaymentMethods.Type paymentType)
    {
        PaymentType = paymentType;

    }
    public static PaymentMethods Find(Type type)
    {

        switch (type)
        {
            case Type.CASH:
                return new Cash();
            case Type.SINPE:
                return new Sinpe();
            default:
                throw new ArgumentException("Invalid payment method type.");
        }
    }
     public static PaymentMethods SetPaymentType(Type type)
    {
        return Find(type);
    }
}
public sealed class Sinpe : PaymentMethods
{
    public Sinpe() : base(PaymentMethods.Type.SINPE)
    {

    }
}
public sealed class Cash : PaymentMethods
{
    public Cash() : base(PaymentMethods.Type.CASH)
    {

    }
}