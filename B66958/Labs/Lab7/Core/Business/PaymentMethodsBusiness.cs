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
        }else
        {
            throw new BusinessException("The method provided could not be found");
        }
    }

    public async Task DisablePaymentMethodAsync(PaymentMethods.Type paymentMethodType)
    {
        var paymentMethod = PaymentMethods.Find(paymentMethodType);
        if (paymentMethod != null)
        {
            paymentMethod.Disable();
            await db.UpdatePaymentMethodAsync(paymentMethod);
        }else
        {
            throw new BusinessException("The method provided could not be found");
        }
    }

    public IEnumerable<PaymentMethods> GetAllPaymentMethods()
    {
        var paymentMethods = db.GetPaymentMethods().Where(method => method.IsEnabled).ToList();
        if(paymentMethods.Count == 0) throw new BusinessException("No payment methods at the moment");
        return paymentMethods;
    }
}