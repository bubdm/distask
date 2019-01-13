using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.TaskDispatchers;
using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.Logging;

namespace Distask.TaskDispatchers.Routing
{
    public sealed class RandomizedRouter : Router
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Millisecond);

        public RandomizedRouter(ILogger<RandomizedRouter> logger)
            : base(logger)
        { }

        protected override Task<IBrokerClient> GetRoutedClientCoreAsync(string group, IEnumerable<IBrokerClient> availableClients)
        {
            var numOfClients = availableClients.Count();
            if (numOfClients == 0)
            {
                return null;
            }

            return Task.FromResult(numOfClients == 1 ? availableClients.First() : availableClients.ElementAt(rnd.Next(numOfClients)));
        }
    }
}
