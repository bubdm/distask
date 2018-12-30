using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Distask
{
    public abstract class ServiceRunner<TServiceHost>
        where TServiceHost : class, IServiceHost
    {
        private string[] args;

        protected virtual string EnvironmentVariablePrefix => null;

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

        protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IHostedService, TServiceHost>();
        }

        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            logging.AddConsole();
        }

        public virtual async Task RunAsync(string[] args)
        {
            this.args = args;
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(this.ConfigureHostConfiguration)
                .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
                .ConfigureServices(this.ConfigureServices)
                .ConfigureLogging(this.ConfigureLogging);

            await hostBuilder.RunConsoleAsync();
        }
    }
}
