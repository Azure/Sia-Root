using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public abstract class CertificateRetriever : IHttpClientFactory, IDisposable
    {
        protected CertificateRetriever(ILogger logger)
        {
            this.logger = logger;
        }
        private X509Certificate2 _cert;
        private HttpClient _client;


        protected abstract Task<X509Certificate2> RetrieveCertificateAsync();
        protected ILogger logger { get; }

        public async Task<HttpClient> GetClientAsync()
        {
            if(_client is null)
            {
                if (_cert is null)
                {
                    _cert = await RetrieveCertificateAsync();
                    logger.LogDebug($"Retrieved Certificate with thumbprint {_cert.Thumbprint}");
                }
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(_cert);
                
                _client = new HttpClient(handler);
                logger.LogDebug($"Created client with certificate with thumbprint {_cert.Thumbprint}");
            }
            return _client;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
                _cert.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
