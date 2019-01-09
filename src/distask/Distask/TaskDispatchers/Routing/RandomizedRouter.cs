using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.TaskDispatchers;
using Distask.TaskDispatchers.Client;

namespace Distask.TaskDispatchers.Routing
{
    public sealed class RandomizedRouter : Router
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Millisecond);

        protected override Task<IBrokerClient> GetRoutedClientCoreAsync(string group, IEnumerable<IBrokerClient> availableClients)
        {
            var numOfClients = availableClients.Count();
            if (numOfClients > 0)
            {
                var index = rnd.Next(numOfClients);
                return Task.FromResult(availableClients.ElementAt(index));
            }

            return null;
        }
    }
}
