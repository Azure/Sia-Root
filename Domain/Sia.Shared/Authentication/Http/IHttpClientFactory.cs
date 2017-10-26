using System.Net.Http;

namespace Sia.Shared.Authentication
{
    public interface IHttpClientFactory
    {
        HttpClient GetClient();
    }
}
