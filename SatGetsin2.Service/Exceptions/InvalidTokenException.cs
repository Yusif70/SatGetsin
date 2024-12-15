namespace SatGetsin2.Service.Exceptions
{
    public class InvalidTokenException : Exception
    {
        private static readonly string _message = "Token is invalid";
        public InvalidTokenException() : base(_message) { }
        public InvalidTokenException(string message) : base(message) { }
    }
}
