using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Sia.Shared.Authentication
{
    public abstract class CertificateRetriever : IHttpClientFactory
    {
        protected CertificateRetriever(ILogger logger)
        {
            _logger = logger;
        }
        private X509Certificate2 _cert;
        private HttpClient _client;
        public X509Certificate2 Certificate
        {
            get
            {
                if (_cert == null)
                {
                    _cert = RetrieveCertificate();
                }
                return _cert;
            }
        }
        protected abstract X509Certificate2 RetrieveCertificate();
        protected ILogger _logger { get; }

        public HttpClient GetClient()
        {
            if(_client is null)
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(Certificate);
                
                _client = new HttpClient(handler);
                _logger.LogDebug($"Created client with certificate {Certificate.FriendlyName}");
            }
            return _client;
        }
    }
}
