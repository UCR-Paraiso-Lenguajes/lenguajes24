namespace ApiLab7;

public class PaymentMethodsBusiness
{
    private Db db;

    public PaymentMethodsBusiness()
    {
        db = Db.Instance;
    }

    public async Task EnablePaymentMethodAsync(PaymentMethods.Type paymentMethodType)
    {
        var paymentMethod = PaymentMethods.Find(paymentMethodType);
        if (paymentMethod != null)
        {
            paymentMethod.Enable();
            await db.UpdatePaymentMethodAsync(paymentMethod);
        }
    }

    public async Task DisablePaymentMethodAsync(PaymentMethods.Type paymentMethodType)
    {
        var paymentMethod = PaymentMethods.Find(paymentMethodType);
        if (paymentMethod != null)
        {
            paymentMethod.Disable();
            await db.UpdatePaymentMethodAsync(paymentMethod);
        }
    }

    public IEnumerable<PaymentMethods> GetAllPaymentMethods()
    {
        var paymentMethods = db.GetPaymentMethods();
        return paymentMethods;
    }
}