using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Core.Requests
{
    public abstract class DatabaseOperationHandler<TContext, TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TContext : DbContext
    {
        protected readonly TContext _context;

        protected DatabaseOperationHandler(TContext context)
        {
            _context = context;
        }

        public abstract Task<TResult> Handle(TRequest message, CancellationToken cancellationToken);
    }

    public abstract class DatabaseOperationHandler<TContext, TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
        where TContext : DbContext
    {
        protected readonly TContext _context;

        protected DatabaseOperationHandler(TContext context)
        {
            _context = context;
        }

        public abstract Task Handle(TRequest message, CancellationToken cancellationToken);
    }
}
