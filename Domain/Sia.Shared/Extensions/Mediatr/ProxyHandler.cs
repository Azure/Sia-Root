using MediatR;
using Sia.Shared.Authentication.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Sia.Shared.Protocol;
using Sia.Shared.Exceptions;
using Newtonsoft.Json;
using Sia.Shared.Requests;
using System.Net.Http.Headers;
using System.Threading;
using Sia.Shared.Authentication;

namespace Sia.Shared.Extensions.Mediatr
{
    public abstract class ProxyHandler<TRequest, TResult> : ProxyHandlerBase<TRequest>, IRequestHandler<TRequest, TResult>
        where TRequest : AuthenticatedRequest<TResult>
    {
        protected ProxyHandler(HttpClientLookup clientFactory, string endpointName) 
            : base(clientFactory, endpointName)
        {
        }

        public virtual async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await SendRequest(request, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            var logicalResponse = await ResponseFactory.CreateResponse <TResult>(httpResponse)
                .ConfigureAwait(continueOnCapturedContext: false);
            logicalResponse.ThrowExceptionOnUnsuccessfulStatus();
            return logicalResponse.Value;
        }
    }

    public abstract class ProxyHandler<TRequest> : ProxyHandlerBase<TRequest>, IRequestHandler<TRequest>
        where TRequest : AuthenticatedRequest
    {
        protected ProxyHandler(HttpClientLookup clientFactory, string endpointName)
            : base(clientFactory, endpointName)
        {
        }

        public virtual async Task Handle(TRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await SendRequest(request, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            var logicalResponse = await ResponseFactory.CreateResponse(httpResponse)
                .ConfigureAwait(continueOnCapturedContext: false);
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

        protected virtual async Task<HttpResponseMessage> SendRequest(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var message = new HttpRequestMessage(Method(), RelativeUri(request));
                AddContentToMessage(request, message);
                await AddAuthorizationToMessage(request, message)
                    .ConfigureAwait(continueOnCapturedContext: false);
                var result = await _client
                    .SendAsync(message, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return result;
            }
            catch (HttpRequestException ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
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
            var tokenResult = await request
                .UserContext
                .AcquireTokenAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
            message.Headers.Authorization = new AuthenticationHeaderValue(request.UserContext.AuthConfig.Scheme, tokenResult);
        }

        protected ProxyHandlerBase(HttpClientLookup clientFactory, string endpointName)
        {
            _client = clientFactory.GetClientForEndpoint(endpointName);
        }
    }
}
