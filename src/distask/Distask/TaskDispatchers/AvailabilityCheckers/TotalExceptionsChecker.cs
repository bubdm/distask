using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.Logging;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public sealed class TotalExceptionsChecker : AvailabilityChecker
    {
        private readonly long maximumExceptionCount;

        public TotalExceptionsChecker(ILogger<TotalExceptionsChecker> logger)
            : this(logger, 20)
        { }

        public TotalExceptionsChecker(ILogger<TotalExceptionsChecker> logger, long maximumExceptionCount)
            : base(logger)
        {
            this.maximumExceptionCount = maximumExceptionCount;
        }

        protected override Task<bool> IsAvailableInternalAsync(IBrokerClient client)
        {
            return Task.FromResult(client.State.TotalExceptions <= this.maximumExceptionCount);
        }
    }
}
