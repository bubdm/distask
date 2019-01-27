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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Distask
{
    /// <summary>
    /// Represents the base class for service runners.
    /// </summary>
    /// <typeparam name="TServiceHost">The type of the service host which should be run by the current runner.</typeparam>
    public abstract class ServiceRunner<TServiceHost>
        where TServiceHost : class, IServiceHost
    {
        #region Private Fields

        private string[] args;

        #endregion Private Fields

        #region Protected Properties

        /// <summary>
        /// Gets the environment variable prefix.
        /// </summary>
        /// <value>
        /// The environment variable prefix.
        /// </value>
        protected virtual string EnvironmentVariablePrefix => null;

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        /// Runs the service host asynchronously.
        /// </summary>
        /// <param name="args">The command line arguments used for running the service host.</param>
        /// <returns>A <see cref="Task"/> object which runs the service host.</returns>
        public virtual async Task RunAsync(string[] args)
        {
            this.args = args;
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(this.ConfigureHostConfiguration)
                .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
                .ConfigureServices(this.ConfigureServices)
                .ConfigureLogging(this.ConfigureLogging);

            await ConfigureAdditionalFeatures(hostBuilder).RunConsoleAsync();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual IHostBuilder ConfigureAdditionalFeatures(IHostBuilder hostBuilder)
            => hostBuilder;

        /// <summary>
        /// Configures the application configuration.
        /// </summary>
        /// <param name="context">The context of the host builder.</param>
        /// <param name="config">The configuration.</param>
        protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            var dir = Directory.GetCurrentDirectory();
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: true);
            config.AddJsonFile(
                $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                optional: true);
            config.AddEnvironmentVariables(EnvironmentVariablePrefix);
            if (this.args != null)
            {
                config.AddCommandLine(this.args);
            }
        }

        /// <summary>
        /// Configures the host configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        protected virtual void ConfigureHostConfiguration(IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("hostsettings.json", true);
            config.AddEnvironmentVariables(EnvironmentVariablePrefix);
            if (this.args != null)
            {
                config.AddCommandLine(this.args);
            }
        }

        /// <summary>
        /// Configures the logging.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logging">The logging.</param>
        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            logging.AddConsole();
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="services">The services.</param>
        protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IHostedService, TServiceHost>();
        }

        #endregion Protected Methods
    }
}