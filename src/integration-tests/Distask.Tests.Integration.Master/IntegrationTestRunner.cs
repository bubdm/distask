using System;
using System.Collections.Generic;
using System.Text;
using Distask.TaskDispatchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Distask.TaskDispatchers.Config;
using Distask.TaskDispatchers.Routing;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Microsoft.Extensions.Logging;
using Distask.TaskDispatchers.Client;

namespace Distask.Tests.Integration.Master
{
    public sealed class IntegrationTestRunner : ServiceRunner<IntegrationTestHost>
    {
        public IntegrationTestRunner()
        {
        }

        protected override string EnvironmentVariablePrefix => "DISTASK_INTTEST_";

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            
            base.ConfigureServices(context, services);

            var config = context.Configuration;
            var taskDispatcherConfig = config.GetSection("TaskDispatcher").Get<TaskDispatcherConfiguration>();

            services.AddSingleton(taskDispatcherConfig)
                .AddSingleton<IRouter, RandomizedRouter>()
                .AddSingleton<IAvailabilityChecker>(s => new HealthLevelChecker(s.GetService<ILogger<HealthLevelChecker>>(), BrokerClientHealthLevel.Excellent))
                .AddSingleton<ITaskDispatcher, TaskDispatcher>();
        }
    }
}
