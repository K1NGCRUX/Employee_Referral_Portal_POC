namespace Business_Logic_Layer
{
    public class CustomException : Exception
    {
        public CustomException(string message): base(message) { }
    }
}