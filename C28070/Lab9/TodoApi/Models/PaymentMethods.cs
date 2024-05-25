namespace TodoApi;

public abstract class PaymentMethods {

    public enum Type
    {Efectivo = 0, Sinpe = 1}
    public Type PaymentType { get; set; }
    public PaymentMethods(PaymentMethods.Type paymentType)

    {
        PaymentType = paymentType;

    }
    
    private static Sinpe sinpe=new Sinpe();
    private static Efectivo efectivo=new Efectivo();

    public static PaymentMethods Find(Type type)
    {

        switch (type)
        {
            case Type.Efectivo:
                return efectivo;
            case Type.Sinpe:
                return sinpe;
            default:
                throw new ArgumentException("método de pago inválido");
        }
    }
    
     public static PaymentMethods SetPaymentType(Type type)
    {
        return Find(type);
    }
}
public sealed class Sinpe : PaymentMethods
{
    public Sinpe() : base(PaymentMethods.Type.Sinpe)
    {

    }
}
public sealed class Efectivo : PaymentMethods
{
    public Efectivo() : base(PaymentMethods.Type.Efectivo)
    {

    }
}
