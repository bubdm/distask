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
            RerouteRetryCount = Utils.Constants.DefaultRerouteRetryCount,
            LastRoutedTimeThreshold = TimeSpan.FromSeconds(15)
        };

        public int RerouteRetryCount { get; set; }

        public TimeSpan LastRoutedTimeThreshold { get; set; }

        public ResiliencyConfiguration Resiliency { get; set; }
    }
}
