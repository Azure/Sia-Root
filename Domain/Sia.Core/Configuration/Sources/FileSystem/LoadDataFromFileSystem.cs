using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sia.Core.Configuration.Sources.GitHub;
using Sia.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sia.Core.Configuration.Sources.FileSystem
{
    public static class LoadDataFromFileSystem
    {
        private const string JsonFilePattern = "*.json";
        public static IEnumerable<(FileInfo file, T record)> GetDataFromLocal<T>(
            this IPathConfig config,
            ILogger logger,
            bool recurse = true
        ) where T : class
        {
            FileInfo[] files = null;

            var directory = new DirectoryInfo(config.Path);

            try
            {
                files = recurse
                    ? directory.GetFilesRecursive(JsonFilePattern).ToArray()
                    : directory.GetFiles(JsonFilePattern);
                return files.Select(fileInfo => (file: fileInfo, record: TryDeserialize<T>(fileInfo, logger)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failure to read from path", new[] { config.Path });
            }
            return Enumerable.Empty<(FileInfo file, T record)>();
        }

        public static IEnumerable<FileInfo> GetFilesRecursive(
            this DirectoryInfo directory,
            string pattern
        )
            => directory
                .GetFiles(pattern)
                .Concat(
                    directory
                        .GetDirectories()
                        .SelectMany(subdirectory => subdirectory.GetFilesRecursive(pattern))
                );

        private static T TryDeserialize<T>(FileInfo file, ILogger logger)
            where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(file.FullName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failure when trying to deserialize {file.Name} as json", Array.Empty<object>());
            }
            return null;
        }
    }
}

