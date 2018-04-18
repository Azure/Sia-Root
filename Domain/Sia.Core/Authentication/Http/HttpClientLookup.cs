using Sia.Core.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Sia.Core.Authentication.Http
{
    public class HttpClientLookup
    {
        private ConcurrentDictionary<string, string> _endpointToBaseUrl;
        private ConcurrentDictionary<string, HttpClient> _endpointToHttpClient { get; }

        public HttpClientLookup()
        {
            _endpointToBaseUrl = new ConcurrentDictionary<string, string>();
            _endpointToHttpClient = new ConcurrentDictionary<string, HttpClient>();
        }

        public void RegisterEndpoint(string endpointName, string baseUrl)
            => _endpointToBaseUrl.TryAdd(endpointName, baseUrl);

        public HttpClient GetClientForEndpoint(string endpointName)
        {
            if (_endpointToHttpClient.TryGetValue(endpointName, out HttpClient storedHttpClient)) return storedHttpClient;
            if (!_endpointToBaseUrl.TryGetValue(endpointName, out string baseUrl)) throw new EndpointNotConfiguredException(endpointName);

            var client = HttpClientExtensions.CreateHttpClient(baseUrl);

            _endpointToHttpClient.TryAdd(endpointName, client);

            return client;
        }
    }
}
