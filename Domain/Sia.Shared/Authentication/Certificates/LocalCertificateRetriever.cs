using Microsoft.Extensions.Logging;
using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public class LocalCertificateRetriever
         : CertificateRetriever
    {
        private readonly string _certThumbprint;
        private List<StoreLocation> _storeLocations
            => new List<StoreLocation>
            {
                StoreLocation.CurrentUser,
                StoreLocation.LocalMachine
            };

        public LocalCertificateRetriever(
            string certThumbprint,
            ILoggerFactory loggerFactory
        ) : base(loggerFactory.CreateLogger<LocalCertificateRetriever>())
        {
            _certThumbprint = ThrowIf.NullOrWhiteSpace(certThumbprint, nameof(certThumbprint));
        }

        protected override Task<X509Certificate2> RetrieveCertificateAsync()
        {
            foreach (var location in _storeLocations)
            {
                using (X509Store store = new X509Store("My", location))
                {
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, _certThumbprint, false);
                    if (certificates.Count > 0)
                    {
                        return Task.FromResult(certificates[0]);
                    }
                }

            }
            throw new KeyNotFoundException("Could not find valid certificate");
        }

    }
}
