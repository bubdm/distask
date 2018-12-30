using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Brokers.Config
{
    public sealed class BrokerHostConfig
    {
        public BrokerHostConfig()
        {

        }

        public string Name { get; set; }

        public string Group { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public MasterConfig Master { get; set; }
    }
}
