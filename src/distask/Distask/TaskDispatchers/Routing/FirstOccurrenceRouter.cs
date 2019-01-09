using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.TaskDispatchers;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Client;

namespace Distask.TaskDispatchers.Routing
{
    public class FirstOccurrenceRouter : Router
    {
        protected override Task<IBrokerClient> GetRoutedClientCoreAsync(string group, IEnumerable<IBrokerClient> availableClients)
        {
            return Task.FromResult(availableClients.FirstOrDefault());
        }
    }
}
