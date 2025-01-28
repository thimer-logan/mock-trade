namespace StocksApp.Domain.Exceptions
{
    public class InsufficientSharesException : Exception
    {
        public InsufficientSharesException(string message) : base(message)
        {
        }
    }
}
