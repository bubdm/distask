using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Distask.Contracts;
using Grpc.Core;
using static Distask.Contracts.DistaskService;

namespace Distask.Brokers
{
    public class BrokerClient : IBrokerClient
    {
        #region Private Fields

        private long totalPingCount = 0L;
        private long successPingCount = 0L;

        private readonly Channel channel;
        private readonly DistaskServiceClient wrappedClient;

        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        public BrokerClient(string name, string host, int port, ChannelCredentials channelCredentials)
        {
            this.Name = name;
            this.channel = new Channel(host, port, channelCredentials);
            this.wrappedClient = new DistaskServiceClient(channel);

        }

        #endregion Public Constructors

        #region Private Destructors

        ~BrokerClient()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion Private Destructors

        #region Public Properties

        public string Name { get; }

        public float HealthScore => totalPingCount == 0 ? 100F : successPingCount * 100F / totalPingCount;

        #endregion Public Properties

        // To detect redundant calls

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    channel.ShutdownAsync().Wait();
                }

                disposedValue = true;
            }
        }

        public async Task<DistaskResponse> ExecuteAsync(DistaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            totalPingCount++;
            var response = await wrappedClient.ExecuteAsync(request, cancellationToken: cancellationToken);
            successPingCount++;
            return response;
        }
    }
    #endregion Protected Methods
}
