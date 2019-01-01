using Distask.Brokers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Routing
{
    public interface IRouter
    {
        IBrokerClient GetRoutedClient(string group, IEnumerable<IBrokerClient> clients);
    }
}
