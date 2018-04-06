using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Core.Authentication
{
    public interface IHttpClientFactory
    {
        Task<HttpClient> GetClientAsync();
    }
}
