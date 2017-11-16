using Sia.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Sia.Shared.Authentication.Http
{
    public class HttpClientLookup
    {
        private IDictionary<string, string> _endpointToBaseUrl;
        private IDictionary<string, HttpClient> _endpointToHttpClient { get; }

        public HttpClientLookup()
        {
            _endpointToBaseUrl = new Dictionary<string, string>();
            _endpointToHttpClient = new Dictionary<string, HttpClient>();
        }

        public void RegisterEndpoint(string endpointName, string baseUrl)
            => _endpointToBaseUrl.Add(endpointName, baseUrl);

        public HttpClient GetClientForEndpoint(string endpointName)
        {
            if (_endpointToHttpClient.TryGetValue(endpointName, out HttpClient storedHttpClient)) return storedHttpClient;
            if (!_endpointToBaseUrl.TryGetValue(endpointName, out string baseUrl)) throw new EndpointNotConfiguredException(endpointName);

            var client = HttpClientExtensions.CreateHttpClient(baseUrl);

            _endpointToHttpClient.Add(endpointName, client);

            return client;
        }
    }
}
