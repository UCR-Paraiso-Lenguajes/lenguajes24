namespace Core;

public class BussinessException : Exception
{
    public BussinessException(string? message) : base(message)
    {
    }

}

public class AgreggateException : Exception
{

}