using Sia.Core.Configuration.Sources.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Configuration.Sources.Git
{
    public class GitConfiguration
    {

        public GitSourceConfiguration Source { get; set; }
        public GitAuthConfiguration Auth { get; set; }
    }

    /// <summary>
    /// Configuration of ways to authenticate to git
    /// Only one valid method needs to be provided
    /// </summary>
    public class GitAuthConfiguration
    {
        public GitBasicAuthConfiguration Basic { get; set; }
    }

    public class GitBasicAuthConfiguration
    {
        /// <summary>
        /// Username for authentication via git
        /// May be blank when the password is a
        /// personal access token
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password or Personal Access Token for git
        /// access.
        /// When used with VSTS, see:
        /// https://docs.microsoft.com/en-us/vsts/report/analytics/client-authentication-options?view=vsts#create-a-personal-access-token
        /// </summary>
        public string Password { get; set; }
    }

    public class GitSourceConfiguration : IPathConfig
    {
        public string Repository { get; set; }
        public string Branch { get; set; }
        public string CloneToPath { get; set; }

#pragma warning disable CA1033 // Interface methods should be callable by child types
        // CA1033 indicates 'It is safe to suppress a warning from this rule
        // if an externally visible method is provided that has the same functionality
        // but a different name than the explicitly implemented method.'
        // CloneToPath gives the same functionality.
        string IPathConfig.Path => CloneToPath;
#pragma warning restore CA1033 // Interface methods should be callable by child types
    }
}
