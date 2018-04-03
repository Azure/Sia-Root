namespace Sia.Shared.Exceptions
{
    public class NotFoundException : BaseException
    {
        public override int StatusCode => 404;

        public NotFoundException(string message) : base(message)
        {
        }


        public NotFoundException()
        {
        }

        public NotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
