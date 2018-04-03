using Microsoft.Extensions.Configuration;
using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Authentication
{
    public class KeyVaultConfiguration
    {
        public KeyVaultConfiguration(string clientId, string clientSecret, string vault)
        {
            ClientId = ThrowIf.NullOrWhiteSpace(clientId, nameof(clientId));
            ClientSecret = ThrowIf.NullOrWhiteSpace(clientSecret, nameof(clientSecret));
            Vault = String.Format(secretUriBase, ThrowIf.NullOrWhiteSpace(vault, nameof(vault)));
        }

        private const string secretUriBase = "https://{0}.vault.azure.net";

        public string Vault { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
    }
}
