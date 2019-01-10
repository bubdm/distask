using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Distask.TaskDispatchers.Client;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public sealed class ExceptionCountThresholdChecker : IAvailabilityChecker
    {
        private readonly long threshold;

        public ExceptionCountThresholdChecker()
            : this(20)
        { }

        public ExceptionCountThresholdChecker(long threshold)
        {
            this.threshold = threshold;
        }

        public Task<bool> IsAvailableAsync(IBrokerClient client)
        {
            return Task.FromResult(client.Index.TotalExceptions <= this.threshold);
        }
    }
}
