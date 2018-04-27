using Octokit;
using Sia.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Core.Configuration.Sources.GitHub
{
    public class GitHubConfiguration
    {
        /// <summary>
        /// Information describing the repository to retrieve files from
        /// And how to access that repository
        /// </summary>
        public GitHubSourceConfiguration Source { get; set; }
        /// <summary>
        /// Information needed for KeyVault to retrieve a GitHub Token
        /// stored in a KeyVault instance. Needed only if Token is not
        /// provided in Source
        /// </summary>
        public GitHubTokenRetrievalConfiguration TokenRetrieval { get; set; }
    }
    public class GitHubSourceConfiguration
    {
        /// <summary>
        /// Information describing the repository to retrieve files from
        /// </summary>
        public GitHubRepositoryConfiguration Repository { get; set; }
        /// <summary>
        /// GitHub token to authenticate for programmatic access
        /// See https://help.github.com/articles/creating-a-personal-access-token-for-the-command-line/
        /// </summary>
        public string Token { get; set; }
    }

    public class GitHubRepositoryConfiguration
    {
        public string Name { get; set; }
        public string Owner { get; set; }
    }

    public class GitHubTokenRetrievalConfiguration : KeyVaultConfiguration
    {
        /// <summary>
        /// Name of the token record within the KeyVault instance
        /// </summary>
        public string TokenName { get; set; }
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

        public static Task EnsureValidTokenAsync(
            this GitHubConfiguration config
        ) => config.Source.EnsureValidTokenAsync(config.TokenRetrieval, config.TokenRetrieval.TokenName);


        public static IGitHubClient GetClient(
            this GitHubSourceConfiguration config,
            string applicationName
        ) => new GitHubClient(new ProductHeaderValue(applicationName))
        {
            Credentials = new Credentials(config.Token)
        };
    }
}
