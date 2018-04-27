using Microsoft.Extensions.Configuration;
using Sia.Core.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sia.Core.Authentication
{
    public class KeyVaultConfiguration
    {
        private const string secretUriBase = "https://{0}.vault.azure.net";

        private string vaultUri;
        public string VaultAddressBase
        {
            get
            {
                if (vaultUri == null)
                {
                    vaultUri = string.Format(
                        CultureInfo.InvariantCulture,
                        secretUriBase,
                        VaultName
                    );
                }
                return vaultUri;
            }
        }
        public string VaultName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
