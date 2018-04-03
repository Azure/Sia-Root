using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Exceptions
{
    public class EndpointNotConfiguredException : Exception
    {
        public EndpointNotConfiguredException(string endpointName) 
            : base($"No base URI was configured for ${endpointName}")
        {
        }

        public EndpointNotConfiguredException()
        {
        }

        public EndpointNotConfiguredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
