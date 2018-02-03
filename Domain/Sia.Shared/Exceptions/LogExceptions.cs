using Microsoft.Extensions.Logging;
using Sia.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Exceptions
{
    public static class LogExceptions
    {
        public static void LogUnsuccessfulHttpRequests(this IResponse response, ILogger logger)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            var message = response.Content;

            logger.LogWarning($"Unsuccessful HttpRequest of type {response.StatusCode}: {message}");
        }

    }
}
