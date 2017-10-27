using MediatR;
using Sia.Shared.Authentication;

namespace Sia.Shared.Requests
{
    public abstract class AuthenticatedRequest<T> : IRequest<T>
    {
        protected AuthenticatedRequest(AuthenticatedUserContext userContext)
        {
            UserContext = userContext;
        }

        public AuthenticatedUserContext UserContext { get; private set; }
    }

    public abstract class AuthenticatedRequest : IRequest
    {
        protected AuthenticatedRequest(AuthenticatedUserContext userContext)
        {
            UserContext = userContext;
        }

        public AuthenticatedUserContext UserContext { get; private set; }
    }
}
