using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Distask.Brokers
{
    public class BrokerRunner : ServiceRunner<BrokerHost>
    {
        private readonly IEnumerable<Type> taskTypes;

        public BrokerRunner(IEnumerable<Type> taskTypes)
        {
            this.taskTypes = taskTypes;
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            base.ConfigureServices(context, services);
            foreach (var taskType in this.taskTypes)
            {
                services.AddSingleton(typeof(BrokerTask), taskType);
            }

            services.AddSingleton(serviceProvider => new Broker(serviceProvider.GetServices<BrokerTask>(), serviceProvider.GetService<ILoggerFactory>()));
        }

        protected override string EnvironmentVariablePrefix => "BROKERHOST_";
    }
}
