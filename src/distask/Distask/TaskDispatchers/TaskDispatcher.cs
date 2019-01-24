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
using System.Collections.Generic;
using Polly.CircuitBreaker;

namespace Distask.TaskDispatchers
{
    public class TaskDispatcher : DistaskRegistrationServiceBase, ITaskDispatcher
    {

        #region Private Fields

        private readonly IAvailabilityChecker availabilityChecker;
        private readonly ConcurrentDictionary<string, List<IBrokerClient>> brokerClients = new ConcurrentDictionary<string, List<IBrokerClient>>();
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

        public TaskDispatcher(int port, ILogger<TaskDispatcher> logger, IRouter router, IAvailabilityChecker availabilityChecker)
            : this(TaskDispatcherConfiguration.AnyAddress(port), logger, router, availabilityChecker)
        { }

        public TaskDispatcher(TaskDispatcherConfiguration config, ILogger<TaskDispatcher> logger, IRouter router, IAvailabilityChecker availabilityChecker)
        {
            this.Configuration = config;
            this.logger = logger;
            this.router = router;
            this.availabilityChecker = availabilityChecker;

            this.recycleTimer = new System.Timers.Timer(this.Configuration.RecyclingConfiguration.Interval.TotalMilliseconds);
            this.recycleTimer.Elapsed += RecycleTimer_Elapsed;

            retryPolicy = Policy.Handle<Exception>()
                .OrResult<DistaskResponse>(r => r.Status != Contracts.StatusCode.Success)
                .RetryAsync(config.BrokerClientConfiguration.RerouteRetryCount);

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

        public event EventHandler<BrokerClientDisposedEventArgs> BrokerClientDisposed;

        public event EventHandler<BrokerClientRecycledEventArgs> BrokerClientRecycled;

        public event EventHandler<BrokerClientRegisteredEventArgs> BrokerClientRegistered;

        public event EventHandler<RecyclingEventArgs> RecyclingCompleted;

        public event EventHandler<RecyclingEventArgs> RecyclingStarted;

        #endregion Public Events

        #region Public Properties

        public TaskDispatcherConfiguration Configuration { get; }

        #endregion Public Properties

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
                            try
                            {
                                var parameters = new RepeatedField<string> { requestMessage.Parameters };
                                var distaskRequest = new DistaskRequest { TaskName = requestMessage.TaskName };
                                distaskRequest.Parameters.AddRange(requestMessage.Parameters);
                                return await client.ExecuteAsync(distaskRequest, ct);
                            }
                            catch (BrokenCircuitException bce) when (bce.InnerException != null && bce.InnerException is RpcException rpcEx && rpcEx.StatusCode == Grpc.Core.StatusCode.Unavailable)
                            {
                                // If the exception was RpcException and the status code was Unavailable, we assume that
                                // the connection to the broker has lost. As a result, the broker client will be recycled.
                                client.State.LifetimeState = BrokerClientLifetimeState.Recycled;
                                this.OnBrokerClientRecycled(new BrokerClientRecycledEventArgs(client));

                                throw new ConnectionLostException($"Client '{client}' has been flagged to be recycled because the connection to that client has lost.", rpcEx);
                            }
                        }

                        throw new NoAvailableClientException(group ?? Utils.Constants.DefaultGroupName);

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

            throw new NoAvailableClientException(group ?? Utils.Constants.DefaultGroupName);
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
                if (clients.Where(c => c.State.LifetimeState == BrokerClientLifetimeState.Alive)
                    .Any(c => string.Equals(c.Name, request.Name) || (string.Equals(c.Host, request.Host) && c.Port == request.Port)))
                {
                    return Task.FromResult(RegistrationResponse.AlreadyExists($"The client '{request.Name}' has already been registered."));
                }

                var newBroker = CreateBrokerClient(request.Name, request.Host, request.Port);
                clients.Add(newBroker);
            }
            else
            {
                clients = new List<IBrokerClient>(new[] { CreateBrokerClient(request.Name, request.Host, request.Port) });
                brokerClients.TryAdd(request.Group, clients);
            }

            this.OnBrokerClientRegistered(new BrokerClientRegisteredEventArgs(request.Group, request.Name, request.Host, request.Port));

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
                    this.recycleTimer.Elapsed -= RecycleTimer_Elapsed;
                    this.recycleTimer.Dispose();
                }

                disposedValue = true;
            }
        }

        protected virtual void OnBrokerClientDisposed(BrokerClientDisposedEventArgs e) => this.BrokerClientDisposed?.Invoke(this, e);

        protected virtual void OnBrokerClientRecycled(BrokerClientRecycledEventArgs e) => this.BrokerClientRecycled?.Invoke(this, e);

        protected virtual void OnBrokerClientRegistered(BrokerClientRegisteredEventArgs e) => this.BrokerClientRegistered?.Invoke(this, e);
        protected virtual void OnRecyclingCompleted(RecyclingEventArgs e) => this.RecyclingCompleted?.Invoke(this, e);

        protected virtual void OnRecyclingStarted(RecyclingEventArgs e) => this.RecyclingStarted?.Invoke(this, e);

        #endregion Protected Methods

        #region Private Methods

        private IBrokerClient CreateBrokerClient(string brokerName, string brokerHost, int brokerPort)
        {
            var createdClient = new BrokerClient(brokerName, brokerHost, brokerPort, ChannelCredentials.Insecure, this.Configuration);
            createdClient.Disposed += CreatedClient_Disposed;
            return createdClient;
        }

        private void CreatedClient_Disposed(object sender, BrokerClientDisposedEventArgs e)
        {
            OnBrokerClientDisposed(e);
        }

        private void DisposeBrokerClient(IBrokerClient brokerClient)
        {
            brokerClient.Dispose();
            brokerClient.Disposed -= CreatedClient_Disposed;
        }

        private void RecycleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (sender is System.Timers.Timer timer)
            {
                try
                {
                    timer.Enabled = false;
                    this.OnRecyclingStarted(new RecyclingEventArgs());
                    var recyclingClients = from p in this.brokerClients.SelectMany(c => c.Value)
                                  where p.State.LifetimeState == BrokerClientLifetimeState.Alive &&
                                  (DateTime.UtcNow - p.State.LastRoutedTime >= this.Configuration.BrokerClientConfiguration.LastRoutedTimeThreshold)
                                  select p;
                    foreach(var client in recyclingClients)
                    {
                        client.State.LifetimeState = BrokerClientLifetimeState.Recycled;
                        this.OnBrokerClientRecycled(new BrokerClientRecycledEventArgs(client));
                    }

                    var recycledClients = from p in this.brokerClients.SelectMany(c => c.Value)
                                          where p.State.LifetimeState == BrokerClientLifetimeState.Recycled
                                          select p;
                    foreach(var client in recycledClients)
                    {
                        DisposeBrokerClient(client);
                    }
                }
                finally
                {
                    this.OnRecyclingCompleted(new RecyclingEventArgs());
                    timer.Enabled = true;
                }
            }
        }

        #endregion Private Methods

    }
}