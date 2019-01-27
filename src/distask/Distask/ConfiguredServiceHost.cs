/****************************************************************************
 *           ___      __             __
 *      ____/ (_)____/ /_____ ______/ /__
 *     / __  / / ___/ __/ __ `/ ___/ //_/
 *    / /_/ / (__  ) /_/ /_/ (__  ) ,<
 *    \__,_/_/____/\__/\__,_/____/_/|_|
 *
 * Copyright (C) 2018-2019 by daxnet, https://github.com/daxnet/distask
 * All rights reserved.
 * Licensed under MIT License.
 * https://github.com/daxnet/distask/blob/master/LICENSE
 ****************************************************************************/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Distask
{
    /// <summary>
    /// Represents the service host which has the configuration data.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration data.</typeparam>
    public abstract class ConfiguredServiceHost<TConfig> : ServiceHost
        where TConfig : class, new()
    {
        #region Protected Fields

        protected readonly TConfig options;

        #endregion Protected Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ConfiguredServiceHost{TConfig}"/> class.
        /// </summary>
        /// <param name="applicationLifetime">The life time instance which allows the host to cleanup the resources in a graceful shutdown.</param>
        /// <param name="configuration">The configuration instance from which the configuration data could be retrieved.</param>
        protected ConfiguredServiceHost(IApplicationLifetime applicationLifetime, IConfiguration configuration)
            : base(applicationLifetime)
        {
            this.options = configuration.GetSection(ConfigurationSectionName).Get<TConfig>();
        }

        #endregion Protected Constructors

        #region Protected Properties

        /// <summary>
        /// Gets the name of the configuration section.
        /// </summary>
        protected abstract string ConfigurationSectionName { get; }

        #endregion Protected Properties
    }
}