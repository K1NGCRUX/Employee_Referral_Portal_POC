namespace Business_Logic_Layer.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException(string message) : base(message) { }
}
