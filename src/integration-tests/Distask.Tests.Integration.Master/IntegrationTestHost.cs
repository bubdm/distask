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
        #region Private Fields

        private readonly ILogger logger;
        private readonly ITaskDispatcher taskDispatcher;
        private readonly List<Thread> threads = new List<Thread>();

        #endregion Private Fields

        #region Public Constructors

        public IntegrationTestHost(ITaskDispatcher taskDispatcher, ILogger<IntegrationTestHost> logger, IConfiguration configuration) : base(configuration)
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

            for (var idx = 0; idx < this.options.NumberOfThreads; idx++)
            {
                var thread = new Thread(ThreadProc);
                thread.Start(new ThreadState { CancellationToken = cancellationToken, ThreadIndex = idx });
                threads.Add(thread);
            }

            return Task.CompletedTask;
        }

        private void ThreadProc(object state)
        {
            var threadState = (ThreadState) state;
            while (true)
            {
                if (threadState.CancellationToken.IsCancellationRequested)
                {
                    break;
                }

                logger.LogInformation($"{threadState.ThreadIndex}");
                Thread.Sleep(500);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Stops the integration test host.
            threads.ForEach(t => t.Join(TimeSpan.FromSeconds(10)));
            logger.LogInformation("Integration Test Host stopped successfully.");
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
            }

            base.Dispose(disposing);
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

        }

        #endregion Private Methods

        class ThreadState
        {
            public CancellationToken CancellationToken { get; set; }

            public int ThreadIndex { get; set; }
        }
    }
}
