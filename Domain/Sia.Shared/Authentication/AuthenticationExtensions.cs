using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Sia.Shared.Authentication;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public static class AuthenticationExtensions
    {
        public static async Task<string> AcquireTokenAsync(this AuthenticatedUserContext authenticationInfo)
        {
            //Todo: per-user auth based on delegated identity
            //string userObjectID = (authenticationInfo.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            string userObjectID = "Test";

            var authContext = new AuthenticationContext(authenticationInfo.AuthConfig.Authority, new NaiveSessionCache(userObjectID, authenticationInfo.Session));
            var credential = new ClientCredential(authenticationInfo.AuthConfig.ClientId, authenticationInfo.AuthConfig.ClientSecret);
            try
            {
                var result = await authContext
                    .AcquireTokenSilentAsync(
                        authenticationInfo.AuthConfig.Resource, 
                        credential, 
                        new UserIdentifier(userObjectID, UserIdentifierType.UniqueId)
                    ).ConfigureAwait(continueOnCapturedContext: false);
                return result.AccessToken;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                var result = await authContext
                    .AcquireTokenAsync(authenticationInfo.AuthConfig.Resource, credential)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return result.AccessToken;
            }
        }
    }
}
