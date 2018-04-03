using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Authentication.Certificates
{
    public class CertificateRetrievalException : Exception
    {
        private const string ErrorMessage = "Failure when attempting to retrieve certificate";

        public CertificateRetrievalException()
            :base(ErrorMessage) { }

        public CertificateRetrievalException(Exception ex)
             : base(ErrorMessage, ex) { }

        public CertificateRetrievalException(string message) : base(message)
        {
        }

        public CertificateRetrievalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
