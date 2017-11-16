using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Sia.Shared.Authentication
{
    public class AuthenticatedUserContext
    {
        public AuthenticatedUserContext(ClaimsPrincipal user, ISession session, AzureActiveDirectoryAuthenticationInfo authConfig)
        {
            User = user;
            Session = session;
            AuthConfig = authConfig;
        }

        public ClaimsPrincipal User { get; private set; }
        public ISession Session { get; private set; }
        public AzureActiveDirectoryAuthenticationInfo AuthConfig { get; private set; }
    }
}
