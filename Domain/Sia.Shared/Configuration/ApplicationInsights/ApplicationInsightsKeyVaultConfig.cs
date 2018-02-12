using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Shared.Configuration.ApplicationInsights
{
    public class ApplicationInsightsKeyVaultConfig
    {
        /// <summary>
        /// Vault name where Application Insights instrumentation key to be used is stored.
        /// </summary>
        public string VaultName { get; set; }
        /// <summary>
        /// Name of the secret to retrieve from the named KeyVault instance and use as Application Insights instrumentation key.
        /// </summary>
        public string InstrumentationKeyName { get; set; }
    }
}
