namespace StoreApi.Models
{
 using System;
using System.Collections.Generic;

public abstract class PaymentMethods
{
    public enum Type
    {
        CASH = 0,
        SINPE = 1
    }

    public Type PaymentType { get; set; }

    public PaymentMethods(Type paymentType)
    {
        PaymentType = paymentType;
    }

    private static readonly Dictionary<Type, Func<PaymentMethods>> PaymentMethodsMap = new Dictionary<Type, Func<PaymentMethods>>
    {
        { Type.CASH, () => new Cash() },
        { Type.SINPE, () => new Sinpe() }
    };

    public static PaymentMethods Find(Type type)
    {
        return PaymentMethodsMap.TryGetValue(type, out var createMethod) ? createMethod() : null;
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

}