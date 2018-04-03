using System;

namespace Sia.Shared.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(string message) : base(message)
        {
        }

        public abstract int StatusCode { get; }
    }
}
