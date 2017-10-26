using Sia.Gateway.Authentication;

namespace Sia.Gateway.Requests
{
    public abstract class AuthenticatedRequest
    {
        protected AuthenticatedRequest(AuthenticatedUserContext userContext)
        {
            UserContext = userContext;
        }

        public AuthenticatedUserContext UserContext { get; private set; }
    }
}
