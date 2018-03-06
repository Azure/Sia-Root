using MediatR;
using Microsoft.Extensions.Configuration;
using Sia.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Shared.Extensions.Mediatr
{
    public abstract class HandlerShortCircuit<TRequest, TResponse, TConfig> 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>

    {
        protected readonly TConfig _config;

        protected HandlerShortCircuit(TConfig config)
        {   
            _config = config;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (ShouldRequestContinue(_config))
            {
                return await next();
            }
            return await GenerateMockAsync(request, cancellationToken);
        }

        public abstract bool ShouldRequestContinue(TConfig config);

        public abstract Task<TResponse> GenerateMockAsync(TRequest request, CancellationToken cancellationToken);
        
    }
}
