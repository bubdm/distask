using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.Logging;

namespace Distask.TaskDispatchers.Routing
{
    public sealed class RoundRobinRouter : Router
    {
        private readonly Carrousel car = new Carrousel();

        public RoundRobinRouter(ILogger<RoundRobinRouter> logger) : base(logger)
        {
        }

        protected override Task<IBrokerClient> GetRoutedClientCoreAsync(string group, IEnumerable<IBrokerClient> availableClients)
        {
            var numOfClients = availableClients.Count();
            if (numOfClients == 0)
            {
                return null;
            }

            return Task.FromResult(numOfClients == 1 ? availableClients.First() : availableClients.ElementAt(car.Next(numOfClients)));
        }

        private class Carrousel
        {
            private int counter = -1;

            public int Next(int total)
            {
                Interlocked.Increment(ref counter);
                var index = counter % total;
                Interlocked.CompareExchange(ref counter, -1, int.MaxValue);
                return index;
            }
        }
    }
}
