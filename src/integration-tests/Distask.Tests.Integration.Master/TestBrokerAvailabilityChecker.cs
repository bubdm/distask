using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.Tests.Integration.Master
{
    public class TestBrokerAvailabilityChecker : AvailabilityChecker
    {
        private readonly int idleThresholdMilliseconds;

        public TestBrokerAvailabilityChecker(ILogger logger, int idleThresholdMilliseconds) : base(logger)
        {
            this.idleThresholdMilliseconds = idleThresholdMilliseconds;
        }

        protected override Task<bool> IsAvailableCoreAsync(IBrokerClient client)
        {
            var lastResponse = client.State.LastResponse;

            if (lastResponse?.Status == Contracts.StatusCode.Error && string.Equals(lastResponse?.ErrorMessage, "Temporary Unavailable"))
            {
                return Task.FromResult((DateTime.UtcNow - client.State.LastRoutedTime.Value).TotalMilliseconds > this.idleThresholdMilliseconds);
            }

            return Task.FromResult(true);
        }
    }
}
