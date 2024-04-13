namespace geekstore_api;

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

    private static Sinpe pagoSinpe=new Sinpe();
    private static Cash pagoCash=new Cash();

    public static PaymentMethods Find(PaymentMethods.Type type)
    {
        switch (type)
        {
            case Type.CASH:
                return pagoSinpe;

            case Type.SINPE:
                return pagoCash;

            default:
                throw new ArgumentException("El tipo de pago es incorrecto");
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