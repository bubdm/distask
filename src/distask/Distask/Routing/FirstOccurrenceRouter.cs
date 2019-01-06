using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.TaskDispatchers;

namespace Distask.Routing
{
    public class FirstOccurrenceRouter : IRouter
    {
        public Task<IBrokerClient> GetRoutedClientAsync(string group, IEnumerable<IBrokerClient> clients)
        {
            return Task.FromResult(clients.FirstOrDefault());
        }
    }
}
