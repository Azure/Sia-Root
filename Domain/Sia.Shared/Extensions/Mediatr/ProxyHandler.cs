using MediatR;
using Sia.Shared.Authentication.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Sia.Shared.Protocol;
using Sia.Shared.Exceptions;
using Newtonsoft.Json;
using Sia.Shared.Requests;
using System.Net.Http.Headers;

namespace Sia.Shared.Extensions.Mediatr
{
    public abstract class ProxyHandler<TRequest, TResult> : ProxyHandlerBase<TRequest>, IAsyncRequestHandler<TRequest, TResult>
        where TRequest : AuthenticatedRequest<TResult>
    {
        protected ProxyHandler(HttpClientLookup clientFactory, string endpointName) 
            : base(clientFactory, endpointName)
        {
        }

        public virtual async Task<TResult> Handle(TRequest request)
        {
            var httpResponse = await SendRequest(request);
            var logicalResponse = await Response<TResult>.Create(httpResponse);
            logicalResponse.ThrowExceptionOnUnsuccessfulStatus();
            return logicalResponse.Value;
        }
    }

    public abstract class ProxyHandler<TRequest> : ProxyHandlerBase<TRequest>, IAsyncRequestHandler<TRequest>
        where TRequest : AuthenticatedRequest
    {
        protected ProxyHandler(HttpClientLookup clientFactory, string endpointName)
            : base(clientFactory, endpointName)
        {
        }

        public virtual async Task Handle(TRequest request)
        {
            var httpResponse = await SendRequest(request);
            var logicalResponse = await Response.Create(httpResponse);
            logicalResponse.ThrowExceptionOnUnsuccessfulStatus();
        }
    }

    public abstract class ProxyHandlerBase<TRequest>
        where TRequest: AuthenticatedRequestBase
    {
        protected readonly HttpClient _client;
        protected abstract HttpMethod Method();
        protected abstract string RelativeUri(TRequest request);
        protected abstract object MessageContent(TRequest request);

        protected virtual async Task<HttpResponseMessage> SendRequest(TRequest request)
        {
            var message = new HttpRequestMessage(Method(), RelativeUri(request));
            AddContentToMessage(request, message);
            await AddAuthorizationToMessage(request, message);
            return await _client.SendAsync(message);
        }
        protected virtual void AddContentToMessage(TRequest request, HttpRequestMessage message)
        {
            var content = MessageContent(request);
            if (content is null) return;
            var encodedContent = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(encodedContent, Encoding.UTF8, HttpClientExtensions.JsonMediaType);
            message.Content = httpContent;
        }
        protected virtual async Task AddAuthorizationToMessage(TRequest request, HttpRequestMessage message)
        {
            var tokenResult = await HttpClientExtensions.AcquireTokenAsync(request.UserContext);
            message.Headers.Authorization = new AuthenticationHeaderValue(request.UserContext.AuthConfig.Scheme, tokenResult);
        }

        protected ProxyHandlerBase(HttpClientLookup clientFactory, string endpointName)
        {
            _client = clientFactory.GetClientForEndpoint(endpointName);
        }
    }
}
