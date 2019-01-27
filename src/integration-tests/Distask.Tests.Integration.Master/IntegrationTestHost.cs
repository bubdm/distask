using Distask.TaskDispatchers;
using Distask.TaskDispatchers.Config;
using Distask.Tests.Integration.Master.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
        #region Private Fields

        private readonly ILogger logger;
        private readonly CancellationTokenSource taskCancellationTokenSource = new CancellationTokenSource();
        private readonly ITaskDispatcher taskDispatcher;
        private readonly List<Task> tasks = new List<Task>();
        private int startedState = 0;

        #endregion Private Fields

        #region Public Constructors

        public IntegrationTestHost(ITaskDispatcher taskDispatcher, 
            ILogger<IntegrationTestHost> logger,
            IApplicationLifetime applicationLifetime, IConfiguration configuration) : base(applicationLifetime, configuration)
        {
            this.logger = logger;
            this.taskDispatcher = taskDispatcher;

            this.taskDispatcher.BrokerClientRegistered += TaskDispatcher_BrokerClientRegistered;
            this.taskDispatcher.BrokerClientRecycled += TaskDispatcher_BrokerClientRecycled;
            this.taskDispatcher.BrokerClientDisposed += TaskDispatcher_BrokerClientDisposed;
        }

        #endregion Public Constructors

        #region Protected Properties

        protected override string ConfigurationSectionName => "IntegrationTestHost";

        #endregion Protected Properties

        #region Public Methods

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting Integration Test Host at {DateTime.Now}");
            logger.LogInformation("IntegrationTestHost configuration:");
            logger.LogInformation(ConvertToFormattedJson(this.options));

            logger.LogInformation("TaskDispatcher configuration:");
            logger.LogInformation(ConvertToFormattedJson(this.taskDispatcher.Configuration));

            for (var idx = 0; idx < this.options.NumberOfTasks; idx++)
            {
                var taskId = idx;
                var task = Task.Run(async () =>
                {
                    while (!taskCancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            var result = await taskDispatcher.DispatchAsync("test", new[] { taskId.ToString() }, cancellationToken: taskCancellationTokenSource.Token);
                            // Console.WriteLine(result.Result);
                        }
                        catch (NoAvailableClientException) when (startedState == 0) 
                        {
                            // When current startedState equals to 0, means the integration test host has just started
                            // and there is no broker registered to the host. In this case, we ignore the NoAvailableClientException.
                        }
                        catch(NoAvailableClientException)
                        {
                            // But when startedState doesn't equal to 0, means once there was a broker registered to the host,
                            // the NoAvailableClientException is caused by some of the broker has dropped the connection, in this case,
                            // the error should be logged.
                            logger.LogError("No available client.");
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("Error");
                        }

                        await Task.Delay(1);
                    }
                });

                tasks.Add(task);
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                Task.WaitAll(this.tasks.ToArray(), 5000);
                logger.LogInformation("Integration Test Host stopped successfully.");
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error ocurred when stopping the integration test host.");
            }

            return Task.CompletedTask;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.taskDispatcher.BrokerClientDisposed -= TaskDispatcher_BrokerClientDisposed;
                this.taskDispatcher.BrokerClientRecycled -= TaskDispatcher_BrokerClientRecycled;
                this.taskDispatcher.BrokerClientRegistered -= TaskDispatcher_BrokerClientRegistered;

                this.taskDispatcher.Dispose();

                logger.LogInformation("Integration Test Host disposed successfully.");
            }

            base.Dispose(disposing);
        }

        protected override void OnHostStopping()
        {
            this.taskCancellationTokenSource.Cancel();
        }

        #endregion Protected Methods

        #region Private Methods

        private static string ConvertToFormattedJson(object obj)
                    => JsonConvert.SerializeObject(obj, Formatting.Indented);
        private void TaskDispatcher_BrokerClientDisposed(object sender, TaskDispatchers.Client.BrokerClientDisposedEventArgs e)
        {

        }

        private void TaskDispatcher_BrokerClientRecycled(object sender, TaskDispatchers.Client.BrokerClientRecycledEventArgs e)
        {

        }

        private void TaskDispatcher_BrokerClientRegistered(object sender, TaskDispatchers.Client.BrokerClientRegisteredEventArgs e)
        {
            Interlocked.Exchange(ref this.startedState, 1);
            logger.LogDebug($"Broker has registered: {e.Name}.");
        }

        #endregion Private Methods

    }
}
