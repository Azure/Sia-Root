using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Core.Configuration.Sources.GitHub
{
    public class GitHubRepositoryRetrievalException : Exception
    {
        public GitHubRepositoryRetrievalException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
