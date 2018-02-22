using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Sia.Shared.Authentication;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string requestUri, AuthenticatedUserContext authenticationInfo)
        {
            return await client.SendAsync(requestUri, authenticationInfo, HttpMethod.Get);
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, HttpContent postContent, AuthenticatedUserContext authenticationInfo)
        {
            return await client.SendAsync(requestUri, postContent, authenticationInfo, HttpMethod.Post);
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient client, string requestUri, HttpContent postContent, AuthenticatedUserContext authenticationInfo)
        {
            return await client.SendAsync(requestUri, postContent, authenticationInfo, HttpMethod.Put);
        }

        private static async Task<HttpResponseMessage> SendAsync(this HttpClient client, string requestUri, AuthenticatedUserContext authenticationInfo, HttpMethod method)
        {
            HttpRequestMessage request = await GenerateRequest(requestUri, authenticationInfo, method);

            return await client.SendAsync(request);
        }

        private static async Task<HttpResponseMessage> SendAsync(this HttpClient client, string requestUri, HttpContent postContent, AuthenticatedUserContext authenticationInfo, HttpMethod method)
        {
            HttpRequestMessage request = await GenerateRequest(requestUri, authenticationInfo, method);
            request.Content = postContent;

            return await client.SendAsync(request);
        }

        private static async Task<HttpRequestMessage> GenerateRequest(string requestUri, AuthenticatedUserContext authenticationInfo, HttpMethod method)
        {
            var tokenResult = await authenticationInfo.AcquireTokenAsync();

            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue(authenticationInfo.AuthConfig.Scheme, tokenResult);
            return request;
        }

        public static HttpClient CreateHttpClient(string baseUrl)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));
            return client;
        }

        public const string JsonMediaType = "application/json";
    }
}
