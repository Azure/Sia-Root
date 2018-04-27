using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octokit;
using Sia.Core.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Core.Configuration.Sources.GitHub
{
    public static class LoadDataFromGitHub
    {
        private const string RepositoryLoadErrorMessage = "Failure to retrieve Github repository {0} with owner {1}";

        private static async Task<Repository> GetRepositoryAsync(
            IGitHubClient client,
            GitHubRepositoryConfiguration config,
            ILogger logger
        )
        {
            try
            {
                return await client.Repository
                    .Get(config.Owner, config.Name)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                var errorMessageTokens = new object[] { config.Name, config.Owner };
                logger.LogError(
                    ex,
                    RepositoryLoadErrorMessage,
                    errorMessageTokens
                );
                throw new GitHubRepositoryRetrievalException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        RepositoryLoadErrorMessage,
                        errorMessageTokens),
                    ex);
            }
        }

        public static async Task<IEnumerable<(string filePath, T resultObject)>> GetSeedDataFromGitHub<T>(
            this IGitHubClient client,
            ILogger logger,
            GitHubRepositoryConfiguration config,
            string searchTerm
        )
            where T: class
        {
            var repository = await GetRepositoryAsync(client, config, logger)
                .ConfigureAwait(continueOnCapturedContext: false);
            var request = new SearchCodeRequest(searchTerm, config.Owner, config.Name)
            {
                In = new[] { CodeInQualifier.Path },
                Extension = "json"
            };

            var result = await client.Search
                .SearchCode(request)
                .ConfigureAwait(continueOnCapturedContext: false);

            var recordsToAddTasks = result
                .Items
                .Select(ExtractContents(client, repository))
                .ToArray();

            Task.WaitAll(
                recordsToAddTasks
                    .Select(taskTuple => taskTuple.contentsTask)
                    .ToArray()
            );
            var recordsToAdd = recordsToAddTasks
                .Where(taskTuple => taskTuple.contentsTask.IsCompleted && !taskTuple.contentsTask.IsFaulted)
                .Select(taskTuple => (
                    contents: taskTuple.contentsTask.Result,
                    filePath: taskTuple.filePath)
                );

            LogFileRetrievalFailures(logger, recordsToAddTasks);

            var content = recordsToAdd
                .SelectMany(DeserializeContents<T>(logger))
                .Where(deserialized => !(deserialized.resultObject is null));

            return content;
        }

        private static void LogFileRetrievalFailures(ILogger logger, (Task<IReadOnlyList<RepositoryContent>> contentsTask, string filePath)[] eventTypesToAddTasks)
        {
            foreach (var failedTask
                in eventTypesToAddTasks
                    .Where(taskTuple => !taskTuple.contentsTask.IsCompleted && !taskTuple.contentsTask.IsFaulted))
            {
                if (failedTask.contentsTask.Exception is null)
                {
                    logger.LogError(
                        "Failure when trying to read file contents from {0}",
                        new object[] { failedTask.filePath }
                    );
                }
                else
                {
                    logger.LogError(
                        failedTask.contentsTask.Exception,
                        "Failure when trying to read file contents from {0}",
                        new object[] { failedTask.filePath }
                    );
                }
            }
        }

        private static Func<(IReadOnlyList<RepositoryContent> contents, string filePath), IEnumerable<(string filePath, T resultObject)>> DeserializeContents<T>(ILogger logger)
            where T: class
        => ((IReadOnlyList<RepositoryContent> contents, string filePath) contentTuple)
        => contentTuple.contents.Select(TryDeserialize<T>(logger, contentTuple.filePath));

        private static Func<RepositoryContent, (string filePath, T resultObject)> TryDeserialize<T>(ILogger logger, string filePath)
            where T: class
        {
            (string filePath, T resultObject) tryDeserialize(RepositoryContent content)
            {
                try
                {
                    return (filePath: filePath, resultObject: JsonConvert.DeserializeObject<T>(content.Content));
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Failure when trying to deserialize eventType json from file {0}",
                        new object[] { filePath }
                    );
                    return (filePath: filePath, resultObject: null);
                }
            }
            return tryDeserialize;
        }


        private static Func<SearchCode, (Task<IReadOnlyList<RepositoryContent>> contentsTask, string filePath)> ExtractContents(
            IGitHubClient client,
            Repository repo
        )
        => (SearchCode item) 
        => (contentsTask: client
                .Repository
                .Content
                .GetAllContents(repo.Id, item.Path),
            filePath: item.Path);

        public static void AddSeedDataToDictionary<T>(
            this Dictionary<long, T> index, 
            IEnumerable<T> toAdd)
            where T: IEntity
        {
            foreach (var item in toAdd)
            {
                index.Add(item.Id, item);
            }
        }
    }
}
