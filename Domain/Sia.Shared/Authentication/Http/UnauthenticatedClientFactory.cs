using System.Net.Http;

namespace Sia.Shared.Authentication
{
    public class UnauthenticatedClientFactory : IHttpClientFactory
    {
        HttpClient _httpClient;
        public HttpClient GetClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            return _httpClient;
        }
    }
}
