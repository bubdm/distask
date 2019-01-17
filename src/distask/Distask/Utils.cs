using System;
using System.Collections.Generic;
using System.Text;

namespace Distask
{
    public static class Utils
    {
        public static class Constants
        {
            public const string DefaultGroupName = "default";

            public const string BrokerHostConfigurationSectionName = "distask.broker";

            public const int TaskDispatcherDefaultPort = 5919;

            public const int DefaultRerouteRetryCount = 5;
        }
    }
}
