using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Distask.Brokers;
using Distask.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Distask.Hosting
{
    class SayHelloTask : BrokerTask
    {
        public SayHelloTask(ILogger<SayHelloTask> logger)
            : base(logger)
        { }

        public override string Name => "say-hello";

        protected override Task<DistaskResponse> ExecuteInternalAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default)
        {
            throw new ExecuteException("test");
        }
    }

    class Add2Task : BrokerTask
    {
        public Add2Task(ILogger<Add2Task> logger)
            : base(logger)
        { }

        public override string Name => "add";

        protected override Task<DistaskResponse> ExecuteInternalAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default)
        {
            var p = parameters.ToList();
            var x = Convert.ToInt32(p[0]);
            var y = Convert.ToInt32(p[1]);
            return Task.FromResult(DistaskResponse.Success((x + y).ToString()));
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var brokerRunner = new BrokerRunner(new[] { typeof(SayHelloTask), typeof(Add2Task) });
            await brokerRunner.RunAsync(args);
        }
    }
}
