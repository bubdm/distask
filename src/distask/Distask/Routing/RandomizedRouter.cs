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
            var numOfClients = clients.Count();
            if (numOfClients > 0)
            {
                var index = rnd.Next(numOfClients);
                return Task.FromResult(clients.ElementAt(index));
            }

            return null;
        }
    }
}
