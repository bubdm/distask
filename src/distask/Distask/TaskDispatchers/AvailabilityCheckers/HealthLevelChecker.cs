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
        private readonly HealthLevel minLevel;

        public HealthLevelChecker(ILogger<HealthLevelChecker> logger, HealthLevel minLevel)
            : base(logger)
        {
            this.minLevel = minLevel;
        }

        public override Task<bool> IsAvailableAsync(IBrokerClient client) => Task.FromResult(client.Index.HealthLevel >= minLevel);
    }
}
