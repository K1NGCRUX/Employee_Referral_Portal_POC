namespace Business_Logic_Layer.Exceptions;

public class BadRequestException : Exception
{
        public BadRequestException(string message) : base(message)
            { }
    }

