using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Config
{
    public class BrokerClientConfiguration
    {
        public static readonly BrokerClientConfiguration Default = new BrokerClientConfiguration
        {
            Resiliency = ResiliencyConfiguration.Default,
            RerouteRetryCount = Utils.Constants.DefaultRerouteRetryCount
        };

        public int RerouteRetryCount { get; set; }

        public ResiliencyConfiguration Resiliency { get; set; }
    }
}
