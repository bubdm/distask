using Distask.Brokers;
using Distask.TaskDispatchers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.Routing
{
    public interface IRouter
    {
        Task<IBrokerClient> GetRoutedClientAsync(string group, IEnumerable<IBrokerClient> clients);
    }
}
