using Distask.Brokers;
using Distask.Contracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Distask.Tests.Integration.Broker
{
    internal sealed class TestTask : BrokerTask
    {
        public TestTask(ILogger<TestTask> logger)
            : base(logger)
        { }

        public override string Name => "test";

        protected override Task<DistaskResponse> ExecuteInternalAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default)
        {
            var taskIndex = parameters.FirstOrDefault();
            if (string.IsNullOrEmpty(taskIndex))
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return Task.FromResult(DistaskResponse.Success($"Response to thread {taskIndex}."));
        }
    }
}
