namespace ApiLab7;

public class BusinessException : Exception
{
    public BusinessException(string? message) : base(message)
    {
    }

}