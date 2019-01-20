using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Client
{
    public sealed class BrokerClientDisposedEventArgs : EventArgs
    {
        public BrokerClientDisposedEventArgs(string name, string host, int port)
        {
            this.Name = name;
            this.Host = host;
            this.Port = port;
        }

        public string Name { get; }

        public string Host { get; }
        
        public int Port { get; }

        public override string ToString()
        {
            return $"{Name} - {Host}:{Port}";
        }
    }
}
