namespace Business_Logic_Layer.Exceptions;

public class ConflictException: Exception
{
    public ConflictException(string message) : base(message) { }
}
