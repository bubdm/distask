using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Distask.Brokers;

namespace Distask.Routing
{
    public class FirstOccurrenceRouter : IRouter
    {
        public IBrokerClient GetRoutedClient(string group, IEnumerable<IBrokerClient> clients)
        {
            return clients.FirstOrDefault();
        }
    }
}
