using System;

namespace Sia.Shared.Exceptions
{
    public class ConflictException : BaseException
    {
        public override int StatusCode => 409;

        public ConflictException(string message) : base(message)
        {
        }
        
        public ConflictException()
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
