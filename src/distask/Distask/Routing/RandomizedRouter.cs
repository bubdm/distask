using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.TaskDispatchers;

namespace Distask.Routing
{
    public sealed class RandomizedRouter : IRouter
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Millisecond);

        public Task<IBrokerClient> GetRoutedClientAsync(string group, IEnumerable<IBrokerClient> clients)
        {
            var availableClients = clients.Where(c => c.IsAvailable);

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
