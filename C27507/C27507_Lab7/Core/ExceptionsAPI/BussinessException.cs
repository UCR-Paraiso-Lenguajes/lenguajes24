using System;
namespace Core;

public class BussinessException : Exception{

    public BussinessException(string exceptionMessage) : base(exceptionMessage) { }
    public BussinessException(string exceptionMessage, Exception innerException) : base(exceptionMessage, innerException) { }


}
