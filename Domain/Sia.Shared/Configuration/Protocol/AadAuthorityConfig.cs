using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Configuration.Protocol
{
    public class AadAuthorityConfig
    {
        /// <summary>
        /// String replacement template for the AAD login endpoint to use.
        /// Use with 'ADAL' AuthNService setting on EventUI
        /// Almost always should be "https://login.microsoftonline.com/{0}"
        /// </summary>
        public string AadInstance { get; set; }
        /// <summary>
        /// String replacement template for the AAD login endpoint to use.
        /// Use with 'MSAL' AuthNService setting on EventUI
        /// Almost always should be "https://login.microsoftonline.com/{0}/v2.0"
        /// </summary>
        public string V2AadInstance { get; set; }
        /// <summary>
        /// Tenant to use; should be unique per organization.
        /// Example value: contoso.onmicrosoft.com
        /// </summary>
        public string Tenant { get; set; }
        public string Authority => String.IsNullOrWhiteSpace(AadInstance) || String.IsNullOrWhiteSpace(Tenant)
            ? null
            : String.Format(AadInstance, Tenant);
        public string V2Authority => String.IsNullOrWhiteSpace(V2AadInstance) || String.IsNullOrWhiteSpace(Tenant)
            ? null
            : String.Format(V2AadInstance, Tenant);
    }
}
