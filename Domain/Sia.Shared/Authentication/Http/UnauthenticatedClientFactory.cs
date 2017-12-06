using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public class UnauthenticatedClientFactory : IHttpClientFactory
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
    }
}
