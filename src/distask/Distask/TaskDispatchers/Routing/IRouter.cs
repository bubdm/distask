using Distask.Brokers;
using Distask.TaskDispatchers;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.Routing
{
    public interface IRouter
    {
        Task<IBrokerClient> GetRoutedClientAsync(string group, IEnumerable<IBrokerClient> clients, IAvailabilityChecker checker);
    }
}
