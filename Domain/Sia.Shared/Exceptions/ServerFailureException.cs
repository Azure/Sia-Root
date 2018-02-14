using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Exceptions
{
    public class ServerFailureException : BaseException
    {
        public ServerFailureException(string message) : base(message)
        {
        }

        public override int StatusCode => 500;
    }
}
