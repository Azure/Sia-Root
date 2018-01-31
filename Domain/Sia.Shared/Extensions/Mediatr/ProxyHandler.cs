﻿using MediatR;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sia.Shared.Validation;

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
            var httpResponse = await SendRequest(request, cancellationToken);
            var logicalResponse = await Response<TResult>.Create(httpResponse);
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
            var httpResponse = await SendRequest(request, cancellationToken);
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

        protected virtual async Task<HttpResponseMessage> SendRequest(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var message = new HttpRequestMessage(Method(), RelativeUri(request));
                AddContentToMessage(request, message);
                await AddAuthorizationToMessage(request, message);
                var response = await _client.SendAsync(message, cancellationToken);
                return response;
            }
            catch (HttpRequestException ex)
            {
                var res = new HttpResponseMessage(HttpStatusCode.NotFound);
                Console.WriteLine("Having trouble in ProxyHandler SendRequest ==>", ex);

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(message: "unexpected exception in proxyhandlerbase send request", innerException:ex);
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
            var tokenResult = await HttpClientExtensions.AcquireTokenAsync(request.UserContext);
            message.Headers.Authorization = new AuthenticationHeaderValue(request.UserContext.AuthConfig.Scheme, tokenResult);
        }

        protected ProxyHandlerBase(HttpClientLookup clientFactory, string endpointName)
        {
            _client = clientFactory.GetClientForEndpoint(endpointName);
        }
    }
}
