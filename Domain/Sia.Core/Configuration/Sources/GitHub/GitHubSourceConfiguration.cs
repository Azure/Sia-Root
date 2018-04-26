using Octokit;
using Sia.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Core.Configuration.Sources.GitHub
{
    public class GitHubSourceConfiguration
    {
        public GitHubRepositoryConfiguration Repository { get; set; }
        public string Token { get; set; }
    }

    public class GitHubRepositoryConfiguration
    {
        public string Name { get; set; }
        public string Owner { get; set; }
    }

    public static class GitHubConfigExtensions
    {
        public static async Task EnsureValidTokenAsync(
            this GitHubSourceConfiguration config,
            KeyVaultConfiguration fallbackVaultConfig,
            string tokenName
        )
        {
            if (string.IsNullOrWhiteSpace(config.Token))
            {
                var keyVault = new AzureSecretVault(fallbackVaultConfig);
                config.Token = await keyVault.Get(tokenName).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public static IGitHubClient GetClient(
            this GitHubSourceConfiguration config,
            string applicationName
        ) => new GitHubClient(new ProductHeaderValue(applicationName))
        {
            Credentials = new Credentials(config.Token)
        };
    }
}
