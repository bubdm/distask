using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskService;

namespace Distask.TaskDispatchers.Client
{
    public interface IBrokerClient : IDisposable
    {
        #region Public Events

        event EventHandler<BrokerClientCircuitBrokenEventArgs> CircuitBroken;

        event EventHandler<BrokerClientCircuitHalfOpenEventArgs> CircuitHalfOpen;

        event EventHandler<BrokerClientCircuitResetEventArgs> CircuitReset;

        #endregion Public Events

        #region Public Properties

        string Host { get; }

        /// <summary>
        /// Gets the name of the representing broker.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }
        int Port { get; }

        BrokerClientState State { get; }

        #endregion Public Properties

        #region Public Methods

        Task<DistaskResponse> ExecuteAsync(DistaskRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Public Methods
    }
}
