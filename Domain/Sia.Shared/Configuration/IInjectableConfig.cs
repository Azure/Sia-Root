using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Configuration
{
    public interface IInjectableConfig
    {
        IServiceCollection RegisterMe(IServiceCollection services);
    }

    public static partial class ConfigExtensions
    {
        public static IServiceCollection RegisterConfig(this IServiceCollection services, IInjectableConfig config)
            => config.RegisterMe(services);
    }
}
