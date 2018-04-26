using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Sia.Core.Validation;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sia.Core.Authentication
{
    public class AzureSecretVault
    {
        private readonly KeyVaultConfiguration _config;
        private const string _secretsEndpoint = "/secrets/";

        public AzureSecretVault(KeyVaultConfiguration configuration)
        {
            _config = ThrowIf.Null(configuration, nameof(configuration));
        }

        public async Task<string> Get(string secretName)
        {
            try
            {
                var secret = await GetKeyVaultClient()
                    .GetSecretAsync(_config.VaultAddressBase + _secretsEndpoint + secretName)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return secret.Value;
            }
            catch (KeyVaultErrorException)
            {
                // TODO: Log exception
                return string.Empty;
            }
        }

        public async Task<X509Certificate2> GetCertificateAsync(string certificateName)
        {
            try
            {
                var client = GetKeyVaultClient();
                var cert = await client
                    .GetCertificateAsync(_config.VaultAddressBase, certificateName)
                    .ConfigureAwait(continueOnCapturedContext: false);

                var secretBundle = await client
                    .GetSecretAsync(cert.Sid)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return new X509Certificate2(Convert.FromBase64String(secretBundle.Value));
            }
            catch (KeyVaultErrorException)
            {
                // TODO: Log exception
                return null;
            }
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(_config.ClientId, _config.ClientSecret);
            AuthenticationResult result = await authContext
                .AcquireTokenAsync(resource, clientCred)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }

        private KeyVaultClient GetKeyVaultClient() => new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));

    }

}
