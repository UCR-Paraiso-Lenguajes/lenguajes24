namespace Core;

public class BussinessException :  Exception
{
    public BussinessException(string message) : base(message)
    {
        if (message == null) throw new ArgumentNullException("Messge cannot be null.");
    }
}