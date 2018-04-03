namespace Sia.Shared.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public override int StatusCode => 403;

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
