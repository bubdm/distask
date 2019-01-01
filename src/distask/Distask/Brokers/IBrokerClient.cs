using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskService;

namespace Distask.Brokers
{
    public interface IBrokerClient : IDisposable
    {
        float HealthScore { get; }

        /// <summary>
        /// Gets the name of the representing broker.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        Task<DistaskResponse> ExecuteAsync(DistaskRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
