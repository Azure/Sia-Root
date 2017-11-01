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
            var tokenResult = await AcquireTokenAsync(authenticationInfo);

            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue(authenticationInfo.AuthConfig.Scheme, tokenResult);
            return request;
        }

        public static async Task<string> AcquireTokenAsync(AuthenticatedUserContext authenticationInfo)
        {
            //Todo: per-user auth based on delegated identity
            //string userObjectID = (authenticationInfo.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            string userObjectID = "Test";

            var authContext = new AuthenticationContext(authenticationInfo.AuthConfig.Authority, new NaiveSessionCache(userObjectID, authenticationInfo.Session));
            var credential = new ClientCredential(authenticationInfo.AuthConfig.ClientId, authenticationInfo.AuthConfig.ClientSecret);
            try
            {
                var result = await authContext.AcquireTokenSilentAsync(authenticationInfo.AuthConfig.Resource, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
                return result.AccessToken;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                var result = await authContext.AcquireTokenAsync(authenticationInfo.AuthConfig.Resource, credential);
                return result.AccessToken;
            }
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
