using System;

namespace Sia.Shared.Authentication
{
    public class AzureActiveDirectoryAuthenticationInfo
    {
        public AzureActiveDirectoryAuthenticationInfo(string clientId, string clientSecret, string tenant)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Tenant = tenant;
        }
        /// <summary>
        ///   For the public AAD endpoint, this will be https://login.microsoftonline.com/{0}, but it may be different in China and potentially other sovereign clouds
        /// </summary>
        public string AadInstance => "https://login.microsoftonline.com/{0}";
        /// <summary>
        /// The application ID of the application you're authenticating TO
        /// </summary>
        public string ClientId { get; }
        /// <summary>
        /// A valid secret for the application you're authenticating TO
        /// </summary>
        public string ClientSecret { get; }
        /// <summary>
        /// Tenant name. For Microsoft internal, microsoft.onmicrosoft.com can be used
        /// </summary>
        public string Tenant { get; }
        /// <summary>
        /// Authentication scheme to use. Must match scheme expected by remote resource.
        /// </summary>
        public string Authority => String.Format(AadInstance, Tenant);
        /// <summary>
        /// Authentication scheme to use. Must match scheme expected by remote resource.
        /// </summary>
        public string Scheme => "Bearer";
    }
}
