using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sia.Shared.Extensions.Mediatr
{
    public class RequestLogger<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILoggerFactory _loggerFactory;

        public RequestLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var logger = _loggerFactory.CreateLogger<TRequest>();
            logger.LogInformation($"Handling {typeof(TRequest).Name}");
            var response = await next();
            logger.LogInformation($"Handled {typeof(TRequest).Name}");
            Console.WriteLine($"Handled {typeof(TRequest).Name}");
            return response;
        }
    }
}
