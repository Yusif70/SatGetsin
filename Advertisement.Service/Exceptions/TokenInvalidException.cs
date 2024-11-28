namespace Advertisement.Service.Exceptions
{
    public class TokenInvalidException : Exception
    {
        private static readonly string _message = "Token is invalid";
        public TokenInvalidException() : base(_message) { }
        public TokenInvalidException(string message) : base(message) { }
    }
}
