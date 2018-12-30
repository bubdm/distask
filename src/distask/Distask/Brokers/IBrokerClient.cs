using System;
using System.Collections.Generic;
using System.Text;
using static Distask.Contracts.DistaskService;

namespace Distask.Brokers
{
    public interface IBrokerClient : IDisposable
    {
        /// <summary>
        /// Gets the name of the representing broker.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        DistaskServiceClient Client { get; }
    }
}
