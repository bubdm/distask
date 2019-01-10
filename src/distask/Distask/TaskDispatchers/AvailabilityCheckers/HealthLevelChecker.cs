using Distask.TaskDispatchers.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public sealed class HealthLevelChecker : IAvailabilityChecker
    {
        private readonly HealthLevel minLevel;

        public HealthLevelChecker(HealthLevel minLevel)
        {
            this.minLevel = minLevel;
        }

        public Task<bool> IsAvailableAsync(IBrokerClient client) => Task.FromResult(client.Index.HealthLevel >= minLevel);
    }
}
