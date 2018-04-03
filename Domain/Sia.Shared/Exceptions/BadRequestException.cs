using System;

namespace Sia.Shared.Exceptions
{
    public class BadRequestException : BaseException
    {
        public override int StatusCode => 400;

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException()
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
