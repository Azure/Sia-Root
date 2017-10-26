using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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

        public LocalCertificateRetriever(string certThumbprint)
        {
            _certThumbprint = certThumbprint;
        }


        public override X509Certificate2 Certificate
        {
            get
            {
                if (_cert == null)
                {
                    _cert = GetCertFromStore();
                }
                return _cert;
            }
        }

        private X509Certificate2 GetCertFromStore()
        {
            foreach (var location in _storeLocations)
            {
                using (X509Store store = new X509Store("My", location))
                {
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, _certThumbprint, true);

                    if (certificates.Count > 0)
                        return certificates[0];
                }
            }
            throw new KeyNotFoundException("Could not find valid certificate");
        }

    }
}
