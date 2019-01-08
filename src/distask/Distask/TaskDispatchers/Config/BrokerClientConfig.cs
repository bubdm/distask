using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Config
{
    public class BrokerClientConfig
    {
        public static readonly BrokerClientConfig DefaultBrokerClientConfig = new BrokerClientConfig
        {
            Resiliency = ResiliencyConfig.DefaultResiliencyConfig
        };

        public ResiliencyConfig Resiliency { get; set; }
    }
}
