using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskService;

namespace Distask.TaskDispatchers
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

        string Host { get; }

        int Port { get; }

        bool IsAvailable { get; }

        float HealthScore { get; }

        Task<DistaskResponse> ExecuteAsync(DistaskRequest request, CancellationToken cancellationToken = default(CancellationToken));

        event EventHandler<BrokerClientCircuitBrokenEventArgs> CircuitBroken;

        event EventHandler<BrokerClientCircuitHalfOpenEventArgs> CircuitHalfOpen;

        event EventHandler<BrokerClientCircuitResetEventArgs> CircuitReset;
    }
}
