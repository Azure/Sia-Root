using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Configuration.Protocol
{
    public class CorsConfig
    {
        /// <summary>
        /// List of origins to allow requests from.
        /// </summary>
        public List<string> AcceptableOrigins { get; set; }
    }
}
