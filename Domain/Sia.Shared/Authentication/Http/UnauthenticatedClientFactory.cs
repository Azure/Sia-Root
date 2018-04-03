using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public class UnauthenticatedClientFactory : IHttpClientFactory, System.IDisposable
    {
        HttpClient _httpClient;
        public Task<HttpClient> GetClientAsync()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            return Task.FromResult(_httpClient);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
