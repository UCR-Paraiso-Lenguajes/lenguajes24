
namespace TodoApi.models;
public sealed class Cart
{
    public required List<string> ProductIds { get; set; }
    public required string Address { get; set; }
    public PaymentMethods.Type PaymentMethod { get; set; }


}

public abstract class  CartWithStatus
{

}

internal class CartPendingtoApprove : CartWithStatus
{
    public CartPendingtoApprove()
    {
    }

    public void Approve()
    {
        //TODO falta de implementar
            throw new NotImplementedException("Pending");
    }
}

public class CartApproved : CartWithStatus
{
    public Sale Sale{ get; private set; }
    public CartApproved(Sale sale1)
    {
        if(sale1 == null ) throw new ArgumentException($"{nameof(sale1)}  is required.");

        this.Sale = sale1;
    }

}