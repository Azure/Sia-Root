using MediatR;
using Sia.Core.Authentication;

namespace Sia.Core.Requests
{
    public abstract class AuthenticatedRequest<T> : AuthenticatedRequestBase, IRequest<T>
    {
        protected AuthenticatedRequest(AuthenticatedUserContext userContext) : base(userContext)
        {
        }
    }

    public abstract class AuthenticatedRequest : AuthenticatedRequestBase, IRequest
    {
        protected AuthenticatedRequest(AuthenticatedUserContext userContext) : base(userContext)
        {
        }
    }

    public abstract class AuthenticatedRequestBase
    {
        protected AuthenticatedRequestBase(AuthenticatedUserContext userContext)
        {
            UserContext = userContext;
        }

        public AuthenticatedUserContext UserContext { get; private set; }
    }
}
