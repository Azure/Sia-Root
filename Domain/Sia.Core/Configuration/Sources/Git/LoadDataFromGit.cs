using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Logging;
using Sia.Core.Configuration.Sources.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sia.Core.Configuration.Sources.Git
{
    public static class LoadDataFromGit
    {
        public static IEnumerable<(FileInfo file, T record)> GetDataFromGit<T>(
            this GitConfiguration config,
            ILogger logger
        ) where T : class
        {
            var cloneResult = config.CloneRepoFromConfig();

            return ((IPathConfig)config.Source).GetDataFromLocal<T>(logger);
        }
        public static CloneOptions GetGitOptions(
            this GitAuthConfiguration config
        ) => new CloneOptions()
        {
            CredentialsProvider = (_url, _user, _cred)
                => new UsernamePasswordCredentials()
                {
                    Username = config.Basic.UserName,
                    Password = config.Basic.Password
                }
        };

        public static string CloneRepoFromConfig(
            this GitConfiguration config
        ) => Repository.Clone(
            config.Source.Repository,
            config.Source.CloneToPath,
            config.Auth.GetGitOptions()
        );
    }
}
