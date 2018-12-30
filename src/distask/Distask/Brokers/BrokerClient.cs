using System;
using System.Collections.Generic;
using System.Text;
using Distask.Contracts;
using Grpc.Core;
using static Distask.Contracts.DistaskService;

namespace Distask.Brokers
{
    public class BrokerClient : IBrokerClient
    {
        #region Private Fields

        private readonly Channel channel;

        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        public BrokerClient(string name, string host, int port, ChannelCredentials channelCredentials)
        {
            this.Name = name;
            this.channel = new Channel(host, port, channelCredentials);
            this.Client = new DistaskServiceClient(channel);
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

        public DistaskServiceClient Client { get; }
        public string Name { get; }

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

        #endregion Protected Methods
    }
}
