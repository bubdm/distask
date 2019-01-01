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

namespace Distask.Masters
{
    public class Master : DistaskRegistrationServiceBase, IMaster
    {
        private readonly Server registrationServer;
        private readonly ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>> brokerClients = new ConcurrentDictionary<string, ConcurrentBag<IBrokerClient>>();

        public Master()
            : this(MasterConfig.AnyAddressDefaultPort)
        { }

        public Master(int port)
            : this(MasterConfig.AnyAddress(port))
        { }

        public Master(MasterConfig config)
        {
            this.registrationServer = new Server
            {
                Services = { BindService(this) },
                Ports = { new ServerPort(config.Host, config.Port, ServerCredentials.Insecure) }
            };

            this.registrationServer.Start();
        }

        public async Task<ResponseMessage> ExecuteAsync(RequestMessage requestMessage,
            string group = Utils.Constants.DefaultGroupName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (brokerClients.TryGetValue(group, out var clients) &&
                clients.Count > 0)
            {
                // TODO: Apply the routing strategy, currently it is only taking the first
                // client to serve the request.
                var client = clients.First();

                Console.WriteLine(client.HealthScore);

                var parameters = new RepeatedField<string> { requestMessage.Parameters };
                var distaskRequest = new DistaskRequest { TaskName = requestMessage.TaskName };
                distaskRequest.Parameters.AddRange(requestMessage.Parameters);

                var distaskResponse = await client.ExecuteAsync(distaskRequest);

                return distaskResponse.ToResponseMessage();
            }
            else
            {
                return ResponseMessage.Error($"Cannot find a client from the group '{group}'.");
            }
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

        ~Master()
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
