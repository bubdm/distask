using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distask
{
    public abstract class ConfiguredServiceHost<TConfig> : ServiceHost
        where TConfig : class, new()
    {
        protected readonly TConfig options;

        protected ConfiguredServiceHost(IConfiguration configuration)
        {
            this.options = configuration.GetSection(ConfigurationSectionName).Get<TConfig>();
        }

        protected abstract string ConfigurationSectionName { get; }
    }
}
