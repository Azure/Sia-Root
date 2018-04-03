using System;

namespace Sia.Shared.Exceptions
{
    public abstract class BaseException : Exception
    {
        public abstract int StatusCode { get; }

        public BaseException(string message) : base(message)
        {
        }
        
        public BaseException()
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
