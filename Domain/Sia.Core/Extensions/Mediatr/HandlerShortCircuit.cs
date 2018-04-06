using MediatR;
using Microsoft.Extensions.Configuration;
using Sia.Core.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Core.Extensions.Mediatr
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
                return await next()
                .ConfigureAwait(continueOnCapturedContext: false); ;
            }
            return await GenerateMockAsync(request, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public abstract bool ShouldRequestContinue(TConfig config);

        public abstract Task<TResponse> GenerateMockAsync(TRequest request, CancellationToken cancellationToken);
        
    }
}
