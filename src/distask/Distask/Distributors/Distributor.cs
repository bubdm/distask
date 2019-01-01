using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.Contracts;
using Grpc.Core;
using static Distask.Contracts.DistaskRegistrationService;
using Google.Protobuf.Collections;
using Distask.Routing;

namespace Distask.Distributors
{
    public class Distributor : DistaskRegistrationServiceBase, IDistributor
    {
        private readonly DistributorConfig config;
        private readonly IRouter router;
        private readonly Server registrationServer;
        private readonly ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>> brokerClients = new ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>>();

        public Distributor()
            : this(DistributorConfig.AnyAddressDefaultPort, new FirstOccurrenceRouter())
        { }

        public Distributor(int port)
            : this(DistributorConfig.AnyAddress(port), new FirstOccurrenceRouter())
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

                if (clients.Count == 1)
                {
                    // If only one client has been assigned to the given group, there is no
                    // need to do a routing since there is only one choice.
                    var client = clients.First();
                    var retryCnt = 0;
                    while (retryCnt < config.RetryCount)
                    {
                        try
                        {
                            var distaskResponse = await client.ExecuteAsync(distaskRequest, cancellationToken);
                            return distaskResponse.ToResponseMessage();
                        }
                        catch
                        {
                            // log
                        }

                        retryCnt++;
                    }

                    
                }
                else
                {
                    // If there are more than 2 clients have been assigned to the given group, it should
                    // use the routing feature to find a proper client to serve the request.
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
                            }
                        }

                        retryCnt++;
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
                //var existingClient = clients.FirstOrDefault(c => string.Equals(c.Name, request.Name));
                //if (existingClient != null)
                //{
                //    existingClient.Dispose();
                //}

                //var client = new BrokerClient(request.Name, request.Host, request.Port, ChannelCredentials.Insecure);
                //clients.Add(client);
            }
            else
            {
                clients = new ConcurrentBag<IBrokerClient>(new[] { new BrokerClient(request.Name, request.Host, request.Port, ChannelCredentials.Insecure) });
                brokerClients.TryAdd(request.Group, clients);
            }

            return Task.FromResult(RegistrationResponse.Success());
        }

        #region IDisposable Support
        private bool disposedValue = false;

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

        ~Distributor()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
