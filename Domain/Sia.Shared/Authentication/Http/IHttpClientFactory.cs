using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public interface IHttpClientFactory
    {
        Task<HttpClient> GetClientAsync();
    }
}
