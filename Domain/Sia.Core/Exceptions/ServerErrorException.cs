using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Exceptions
{
    public class ServerErrorException : BaseException
    {
        private const string ServerErrorMessage = "The server encountered an unexpected condition which prevented it from fulfilling the request.";
        public ServerErrorException()
            : base(ServerErrorMessage)
        { }
        public override int StatusCode => 500;
    }
}
