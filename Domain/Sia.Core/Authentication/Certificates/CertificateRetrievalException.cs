using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Authentication.Certificates
{
    public class CertificateRetrievalException : Exception
    {
        private const string ErrorMessage = "Failure when attempting to retrieve certificate";
        public CertificateRetrievalException()
            :base(ErrorMessage) { }

        public CertificateRetrievalException(Exception ex)
             : base(ErrorMessage, ex) { }
    }
}
