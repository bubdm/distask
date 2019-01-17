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

using Distask.Brokers;
using Distask.Contracts;
using Distask.TaskDispatchers.Config;
using Google.Protobuf.Collections;
using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskRegistrationService;
using Polly;
using Polly.Retry;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Routing;
using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.Logging;

namespace Distask.TaskDispatchers
{
    public class TaskDispatcher : DistaskRegistrationServiceBase, ITaskDispatcher
    {

        #region Private Fields

        private readonly IAvailabilityChecker availabilityChecker;
        private readonly ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>> brokerClients = new ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>>();
        private readonly TaskDispatcherConfiguration config;
        private readonly ILogger logger;
        private readonly System.Timers.Timer recycleTimer;
        private readonly Server registrationServer;
        private readonly RetryPolicy<DistaskResponse> retryPolicy;
        private readonly IRouter router;
        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        public TaskDispatcher(ILogger<TaskDispatcher> logger, IRouter router, IAvailabilityChecker availabilityChecker)
            : this(TaskDispatcherConfiguration.AnyAddressDefaultPort, logger, router, availabilityChecker)
        { }

        public TaskDispatcher(int port, ILogger<TaskDispatcher> logger, IRouter router,  IAvailabilityChecker availabilityChecker)
            : this(TaskDispatcherConfiguration.AnyAddress(port), logger, router, availabilityChecker)
        { }

        public TaskDispatcher(TaskDispatcherConfiguration config, ILogger<TaskDispatcher> logger, IRouter router, IAvailabilityChecker availabilityChecker)
        {
            this.config = config;
            this.logger = logger;
            this.router = router;
            this.availabilityChecker = availabilityChecker;

            this.recycleTimer = new System.Timers.Timer(this.config.RecyclingConfiguration.Interval.TotalMilliseconds);
            this.recycleTimer.Elapsed += RecycleTimer_Elapsed;

            retryPolicy = Policy.Handle<Exception>()
                .OrResult<DistaskResponse>(r => r.Status != Contracts.StatusCode.Success)
                .RetryAsync(config.BrokerClientConfiguration.RerouteRetryCount, new Action<DelegateResult<DistaskResponse>, int>(this.OnRetry));

            this.registrationServer = new Server
            {
                Services = { BindService(this) },
                Ports = { new ServerPort(config.Host, config.Port, ServerCredentials.Insecure) }
            };

            this.registrationServer.Start();
            this.recycleTimer.Start();
            logger.LogDebug("Registration service started successfully.");
        }

        #endregion Public Constructors

        #region Private Destructors

        ~TaskDispatcher()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion Private Destructors

        #region Public Events

        public event EventHandler<BrokerClientRegisteredEventArgs> BrokerClientRegistered;

        #endregion Public Events

        #region Public Methods

        public async Task<ResponseMessage> DispatchAsync(RequestMessage requestMessage,
                    string group = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (brokerClients.TryGetValue(group ?? Utils.Constants.DefaultGroupName, out var clients) &&
                clients.Count > 0)
            {
                try
                {
                    var result = await this.retryPolicy.ExecuteAsync(async (ct) =>
                    {
                        var client = await router.GetRoutedClientAsync(group, clients, this.availabilityChecker);
                        if (client != null)
                        {
                            var parameters = new RepeatedField<string> { requestMessage.Parameters };
                            var distaskRequest = new DistaskRequest { TaskName = requestMessage.TaskName };
                            distaskRequest.Parameters.AddRange(requestMessage.Parameters);
                            return await client.ExecuteAsync(distaskRequest, cancellationToken);
                        }

                        throw new TaskDispatchException("No broker available to serve the request.");

                    }, cancellationToken);

                    return result.ToResponseMessage();
                }
                catch (TaskDispatchException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new TaskDispatchException("Failed to distribute the task, please refer to the inner exception for details.", ex);
                }
            }

            throw new TaskDispatchException("No broker available to serve the request.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override Task<RegistrationResponse> Register(RegistrationRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Group))
            {
                return Task.FromResult(RegistrationResponse.Error("The Group has not been specified in the broker registration request."));
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                return Task.FromResult(RegistrationResponse.Error("The Name has not been specified in the broker registration request."));
            }

            if (brokerClients.TryGetValue(request.Group, out var clients))
            {
                if (clients.Any(c => string.Equals(c.Name, request.Name) || (string.Equals(c.Host, request.Host) && c.Port == request.Port)))
                {
                    return Task.FromResult(RegistrationResponse.AlreadyExists($"The client '{request.Name}' has already been registered."));
                }

                var newBroker = CreateBrokerClient(request.Name, request.Host, request.Port);
                clients.Add(newBroker);
            }
            else
            {
                clients = new ConcurrentBag<IBrokerClient>(new[] { CreateBrokerClient(request.Name, request.Host, request.Port) });
                brokerClients.TryAdd(request.Group, clients);
            }

            this.OnBrokerRegistered(new BrokerClientRegisteredEventArgs(request.Group, request.Name, request.Host, request.Port));

            return Task.FromResult(RegistrationResponse.Success());
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Shuts down the registration server.
                    this.registrationServer.ShutdownAsync().Wait();

                    // Dispose all clients.
                    foreach (var brokerClient in brokerClients.SelectMany(c => c.Value))
                    {
                        this.DisposeBrokerClient(brokerClient);
                    }

                    // Stops the recycling timer.
                    this.recycleTimer.Stop();
                    this.recycleTimer.Dispose();
                }

                disposedValue = true;
            }
        }

        protected virtual void OnBrokerRegistered(BrokerClientRegisteredEventArgs e) => this.BrokerClientRegistered?.Invoke(this, e);

        #endregion Protected Methods

        #region Private Methods

        private IBrokerClient CreateBrokerClient(string brokerName, string brokerHost, int brokerPort)
        {
            var createdClient = new BrokerClient(brokerName, brokerHost, brokerPort, ChannelCredentials.Insecure, this.config);
            createdClient.CircuitBroken += CreatedClient_CircuitBroken;
            createdClient.CircuitHalfOpen += CreatedClient_CircuitHalfOpen;
            createdClient.CircuitReset += CreatedClient_CircuitReset;

            return createdClient;
        }

        private void CreatedClient_CircuitBroken(object sender, BrokerClientCircuitBrokenEventArgs e)
        {

        }

        private void CreatedClient_CircuitHalfOpen(object sender, BrokerClientCircuitHalfOpenEventArgs e)
        {

        }

        private void CreatedClient_CircuitReset(object sender, BrokerClientCircuitResetEventArgs e)
        {

        }

        private void DisposeBrokerClient(IBrokerClient brokerClient)
        {
            brokerClient.CircuitBroken -= this.CreatedClient_CircuitBroken;
            brokerClient.CircuitHalfOpen -= this.CreatedClient_CircuitHalfOpen;
            brokerClient.CircuitReset -= this.CreatedClient_CircuitReset;

            brokerClient.Dispose();
        }

        private void OnRetry(DelegateResult<DistaskResponse> delegateResult, int count)
        {

        }

        private void RecycleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (sender is System.Timers.Timer timer)
            {
                try
                {
                    timer.Enabled = false;

                }
                finally
                {
                    timer.Enabled = true;
                }
            }
        }

        #endregion Private Methods
    }
}