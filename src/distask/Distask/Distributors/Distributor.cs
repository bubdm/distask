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
using Distask.Routing;
using Google.Protobuf.Collections;
using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskRegistrationService;

namespace Distask.Distributors
{
    public class Distributor : DistaskRegistrationServiceBase, IDistributor
    {
        #region Private Fields

        private readonly ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>> brokerClients = new ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>>();
        private readonly DistributorConfig config;
        private readonly Server registrationServer;
        private readonly IRouter router;
        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        public Distributor()
            : this(DistributorConfig.AnyAddressDefaultPort, new FirstOccurrenceRouter())
        { }

        public Distributor(int port)
            : this(DistributorConfig.AnyAddress(port), new FirstOccurrenceRouter())
        { }

        public Distributor(IRouter router)
            : this(DistributorConfig.AnyAddressDefaultPort, router)
        { }

        public Distributor(int port, IRouter router)
            : this(DistributorConfig.AnyAddress(port), router)
        { }

        public Distributor(DistributorConfig config, IRouter router)
        {
            this.config = config;
            this.router = router;
            this.registrationServer = new Server
            {
                Services = { BindService(this) },
                Ports = { new ServerPort(config.Host, config.Port, ServerCredentials.Insecure) }
            };

            this.registrationServer.Start();
        }

        #endregion Public Constructors

        #region Private Destructors

        ~Distributor()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion Private Destructors

        #region Public Events

        public event EventHandler<BrokerRegisteredEventArgs> BrokerRegistered;

        #endregion Public Events

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<ResponseMessage> DistributeAsync(RequestMessage requestMessage,
                    string group = Utils.Constants.DefaultGroupName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (brokerClients.TryGetValue(group, out var clients) &&
                clients.Count > 0)
            {
                var parameters = new RepeatedField<string> { requestMessage.Parameters };
                var distaskRequest = new DistaskRequest { TaskName = requestMessage.TaskName };
                distaskRequest.Parameters.AddRange(requestMessage.Parameters);
                var retryCnt = 0;
                while (retryCnt < config.RetryCount)
                {
                    var client = router.GetRoutedClient(group, clients);
                    if (client != null)
                    {
                        try
                        {
                            var distaskResponse = await client.ExecuteAsync(distaskRequest, cancellationToken);
                            return distaskResponse.ToResponseMessage();
                        }
                        catch
                        {
                            // log
                            retryCnt++;
                        }
                    }
                }

                throw new DistributionException("Failed to distribute the task, the routed client was unable to respond in a timely fashion.");
            }

            throw new DistributionException($"No client has been registered to group '{group}' for serving the request.");
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
                if (clients.Any(c => string.Equals(c.Name, request.Name)))
                {
                    return Task.FromResult(RegistrationResponse.Error($"The client '{request.Name}' has already been registered."));
                }

                var newBroker = new BrokerClient(request.Name, request.Host, request.Port, ChannelCredentials.Insecure);
                clients.Add(newBroker);
            }
            else
            {
                clients = new ConcurrentBag<IBrokerClient>(new[] { new BrokerClient(request.Name, request.Host, request.Port, ChannelCredentials.Insecure) });
                brokerClients.TryAdd(request.Group, clients);
            }

            this.OnBrokerRegistered(new BrokerRegisteredEventArgs(request.Group, request.Name, request.Host, request.Port));

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
                        brokerClient.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        protected virtual void OnBrokerRegistered(BrokerRegisteredEventArgs e) => this.BrokerRegistered?.Invoke(this, e);

        #endregion Protected Methods
    }
}