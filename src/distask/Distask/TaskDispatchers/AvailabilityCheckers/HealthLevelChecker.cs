using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public sealed class HealthLevelChecker : AvailabilityChecker
    {
        private readonly BrokerClientHealthLevel minLevel;

        public HealthLevelChecker(ILogger<HealthLevelChecker> logger, BrokerClientHealthLevel minLevel)
            : base(logger)
        {
            this.minLevel = minLevel;
        }

        protected override Task<bool> IsAvailableCoreAsync(IBrokerClient client) => Task.FromResult(client.State.HealthLevel >= minLevel);
    }
}
