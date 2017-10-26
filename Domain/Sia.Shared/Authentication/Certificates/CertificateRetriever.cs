using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Sia.Shared.Authentication
{
    public abstract class CertificateRetriever : IHttpClientFactory
    {
        protected X509Certificate2 _cert;

        public abstract X509Certificate2 Certificate { get; }

        public virtual HttpClient GetClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(Certificate);

            return new HttpClient(handler);
        }
    }
}
