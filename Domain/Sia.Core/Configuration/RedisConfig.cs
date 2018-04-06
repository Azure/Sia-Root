using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Configuration
{
    public class RedisConfig
    {
        public string CacheEndpoint { get; set; }
        public string Password { get; set; }
        public bool IsValid => !String.IsNullOrWhiteSpace(CacheEndpoint)
            && !String.IsNullOrWhiteSpace(Password);
    }
}
