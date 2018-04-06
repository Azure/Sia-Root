using System;
using System.Globalization;

namespace Sia.Core.Authentication
{
    public class AzureActiveDirectoryAuthenticationInfo
    {
        public AzureActiveDirectoryAuthenticationInfo(string resource, string clientId, string clientSecret, string tenant)
        {
            Resource = resource;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Tenant = tenant;
        }

#pragma warning disable CA1822 // Mark members as static (this member may become configurable, see summary, so it should remain a property of an instance)
        /// <summary>
        /// For the public AAD endpoint, this will be https://login.microsoftonline.com/{0}, but it may be different in China and potentially other sovereign clouds
        /// </summary>
        public string AadInstance => "https://login.microsoftonline.com/{0}";
#pragma warning restore CA1822 // Mark members as static
        /// <summary>
        /// The client ID or resource URI of the application you're authenticating TO
        /// </summary>
        public string Resource { get; }
        /// <summary>
        /// The application ID of the application you're authenticating FROM
        /// </summary>
        public string ClientId { get; }
        /// <summary>
        /// A valid secret for the application you're authenticating FROM
        /// </summary>
        public string ClientSecret { get; }
        /// <summary>
        /// Tenant name. For Microsoft internal, microsoft.onmicrosoft.com can be used
        /// </summary>
        public string Tenant { get; }
        /// <summary>
        /// Authentication scheme to use. Must match scheme expected by remote resource.
        /// </summary>
        public string Authority => string.Format(CultureInfo.InvariantCulture, AadInstance, Tenant);

#pragma warning disable CA1822 // Mark members as static (the authentication scheme is a property of an instance and may become configurable)
        /// <summary>
        /// Authentication scheme to use. Must match scheme expected by remote resource.
        /// </summary>
        public string Scheme => "Bearer";
#pragma warning restore CA1822 // Mark members as static
    }
}
