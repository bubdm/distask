using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Distask.TaskDispatchers.Client;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public sealed class ExceptionThresholdChecker : AvailabilityChecker
    {
        private readonly Type exceptionType;
        private readonly TimeSpan? period;
        private readonly long maximumExceptionCount;

        public ExceptionThresholdChecker(ILogger<ExceptionThresholdChecker> logger, Type exceptionType, TimeSpan? period, long maximumExceptionCount)
            : base(logger)
        {
            this.exceptionType = exceptionType;
            this.period = period;
            this.maximumExceptionCount = maximumExceptionCount;
        }

        protected override Task<bool> IsAvailableCoreAsync(IBrokerClient client) 
            => Task.FromResult(client.State.GetExceptions(this.exceptionType, this.period).LongCount() < maximumExceptionCount);
    }
}
