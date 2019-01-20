using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Client
{
    public sealed class BrokerClientRecycledEventArgs : EventArgs
    {
        public BrokerClientRecycledEventArgs(IBrokerClient brokerClient)
        {
            this.BrokerClient = brokerClient;
        }

        public IBrokerClient BrokerClient { get; }

        public override string ToString()
        {
            return $"Broker client: {BrokerClient}";
        }
    }
}
