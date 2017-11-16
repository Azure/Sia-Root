using MediatR;
using Sia.Shared.Authentication;

namespace Sia.Shared.Requests
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
