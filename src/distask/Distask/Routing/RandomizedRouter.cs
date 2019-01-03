using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Distask.Brokers;

namespace Distask.Routing
{
    public sealed class RandomizedRouter : IRouter
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Millisecond);

        public IBrokerClient GetRoutedClient(string group, IEnumerable<IBrokerClient> clients)
        {
            var numOfClients = clients.Count();
            if (numOfClients > 0)
            {
                var index = rnd.Next(numOfClients);
                return clients.ElementAt(index);
            }

            return null;
        }
    }
}
