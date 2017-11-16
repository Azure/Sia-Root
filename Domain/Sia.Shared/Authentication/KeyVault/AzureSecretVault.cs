using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Sia.Shared.Validation;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public interface ISecretVault
    {
        Task<string> Get(string secretName);
    }
    public class AzureSecretVault : ISecretVault
    {
        private readonly KeyVaultConfiguration _config;
        private const string _secretsEndpoint = "/secrets/";
        private const string _keysEndpoint = "/keys/";
        private const string _certificatesEndpoint = "/certificates/";

        public AzureSecretVault(KeyVaultConfiguration configuration)
        {
            _config = ThrowIf.Null(configuration, nameof(configuration));
        }

        public async Task<string> Get(string secretName)
        {
            try
            {
                var secret = await GetKeyVaultClient().GetSecretAsync(_config.Vault + _secretsEndpoint + secretName).ConfigureAwait(false);
                return secret.Value;
            }
            catch (KeyVaultErrorException ex)
            {
                return string.Empty;
            }
        }

        public async Task<X509Certificate2> GetCertificate(string certificateName)
        {
            try
            {
                var cert = await GetKeyVaultClient()
                    .GetCertificateAsync(_config.Vault, certificateName)
                    .ConfigureAwait(false);
                return new X509Certificate2(cert.Cer);
            }
            catch (KeyVaultErrorException ex)
            {
                return null;
            }
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(_config.ClientId, _config.ClientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }

        private KeyVaultClient GetKeyVaultClient() => new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));

    }

}
