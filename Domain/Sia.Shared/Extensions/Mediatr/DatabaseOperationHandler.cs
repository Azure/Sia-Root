using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Sia.Shared.Requests
{
    public abstract class DatabaseOperationHandler<TContext, TRequest, TResult> : IAsyncRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TContext : DbContext
    {
        protected readonly TContext _context;

        protected DatabaseOperationHandler(TContext context)
        {
            _context = context;
        }

        public abstract Task<TResult> Handle(TRequest message);
    }

    public abstract class DatabaseOperationHandler<TContext, TRequest> : IAsyncRequestHandler<TRequest>
        where TRequest : IRequest
        where TContext : DbContext
    {
        protected readonly TContext _context;

        protected DatabaseOperationHandler(TContext context)
        {
            _context = context;
        }

        public abstract Task Handle(TRequest message);
    }
}
