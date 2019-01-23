using Distask.TaskDispatchers;
using Distask.TaskDispatchers.Config;
using Distask.Tests.Integration.Master.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.Tests.Integration.Master
{
    public sealed class IntegrationTestHost : ConfiguredServiceHost<IntegrationTestHostConfig>
    {
        private readonly ILogger logger;
        private readonly List<Thread> threads = new List<Thread>();
        private readonly ITaskDispatcher taskDispatcher;

        public IntegrationTestHost(ITaskDispatcher taskDispatcher, ILogger<IntegrationTestHost> logger, IConfiguration configuration) : base(configuration)
        {
            this.logger = logger;
            this.taskDispatcher = taskDispatcher;
        }

        protected override string ConfigurationSectionName => "IntegrationTestHost";

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting Integration Test Host at {DateTime.Now}");
            logger.LogInformation("IntegrationTestHost configuration:");
            logger.LogInformation(ConvertToFormattedJson(this.options));

            logger.LogInformation("TaskDispatcher configuration:");
            logger.LogInformation(ConvertToFormattedJson(this.taskDispatcher.Configuration));

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Stops the integration test host.
            threads.ForEach(t => t.Join(TimeSpan.FromSeconds(10)));
            logger.LogInformation("Integration Test Host stopped successfully.");
            return Task.CompletedTask;
        }

        private static string ConvertToFormattedJson(object obj)
            => JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}
