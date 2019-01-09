using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Client;

namespace Distask.TaskDispatchers.Routing
{
    public abstract class Router : IRouter
    {
        public async Task<IBrokerClient> GetRoutedClientAsync(string group, IEnumerable<IBrokerClient> clients, IAvailabilityChecker checker)
        {
            var availableClients = new List<IBrokerClient>();
            //Parallel.ForEach(clients, async client =>
            //{
            //    if (await checker.IsAvailableAsync(client))
            //    {
            //        availableClients.Add(client);
            //    }
            //});

            foreach (var client in clients)
            {
                if (await checker.IsAvailableAsync(client))
                {
                    availableClients.Add(client);
                }
            }

            if (availableClients.Count == 0)
            {
                return null;
            }

            return await this.GetRoutedClientCoreAsync(group, availableClients);
        }

        protected abstract Task<IBrokerClient> GetRoutedClientCoreAsync(string group, IEnumerable<IBrokerClient> availableClients);
    }
}
