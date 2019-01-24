using Distask.Brokers;
using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.Tests.Integration.Broker
{
    internal sealed class TestTask : BrokerTask
    {
        public override string Name => "test";

        public override Task<DistaskResponse> ExecuteAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
