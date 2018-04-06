namespace Sia.Core.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public override int StatusCode => 403;
    }
}
