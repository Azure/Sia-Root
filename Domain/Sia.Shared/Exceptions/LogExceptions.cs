using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Exceptions
{
    public class LogExceptions
    {
        private ILogger _logger;

        public LogExceptions(ILogger logger)
        {
            _logger = logger;
        }
    }
}
