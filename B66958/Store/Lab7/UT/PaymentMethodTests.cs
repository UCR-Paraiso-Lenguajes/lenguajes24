using ApiLab7;

namespace UT;

public class PaymentMethodTests
{
    Store store;

    [OneTimeSetUp]
    public void SetUp()
    {
        Db.BuildDb(
            "Data Source=163.178.173.130;User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;"
        );
        store = Store.Instance;
    }

    [Test]
    public void DisablePaymentMethod_DoesNotThrowException()
    {
        PaymentMethodsBusiness paymentMethodsBusiness = new PaymentMethodsBusiness();
        Assert.DoesNotThrowAsync(() => 
        paymentMethodsBusiness.DisablePaymentMethodAsync(PaymentMethods.Type.CASH));
    }

    [Test]
    public void EnablePaymentMethod_DoesNotThrowException()
    {
        PaymentMethodsBusiness paymentMethodsBusiness = new PaymentMethodsBusiness();
        Assert.DoesNotThrowAsync(() => 
        paymentMethodsBusiness.EnablePaymentMethodAsync(PaymentMethods.Type.CASH));
    }

    [Test]
    public void EnablePaymentNonExistentMethod_ThrowsException()
    {
        PaymentMethodsBusiness paymentMethodsBusiness = new PaymentMethodsBusiness();
        Assert.ThrowsAsync<BusinessException>(() => 
        paymentMethodsBusiness.EnablePaymentMethodAsync((PaymentMethods.Type) 3));
    }

    [Test]
    public void DisablePaymentNonExistentMethod_ThrowsException()
    {
        PaymentMethodsBusiness paymentMethodsBusiness = new PaymentMethodsBusiness();
        Assert.ThrowsAsync<BusinessException>(() => 
        paymentMethodsBusiness.EnablePaymentMethodAsync((PaymentMethods.Type) 3));
    }

    [Test]
    public async Task FetchingPaymentMethods_DoesNotThrowException()
    {
        PaymentMethodsBusiness paymentMethodsBusiness = new PaymentMethodsBusiness();
        await paymentMethodsBusiness.EnablePaymentMethodAsync(PaymentMethods.Type.CASH);
        await paymentMethodsBusiness.EnablePaymentMethodAsync(PaymentMethods.Type.SINPE);
        var methods = paymentMethodsBusiness.GetAllPaymentMethods();
        CollectionAssert.IsNotEmpty(methods);
    }
}