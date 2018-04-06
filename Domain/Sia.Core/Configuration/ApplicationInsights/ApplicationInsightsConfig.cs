using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Configuration.ApplicationInsights
{
    public class ApplicationInsightsConfig
    {
        /// <summary>
        /// Instrumentation key to be used by application insights.
        /// Can be set in environment config or populated from key vault.
        /// </summary>
        public string InstrumentationKey { get; set; }
    }
}
