using Distask.Brokers;
using System;
using System.Threading.Tasks;

namespace Distask.Tests.Integration.Broker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var brokerRunner = new BrokerRunner(new[] { typeof(TestTask) });
            await brokerRunner.RunAsync(args);
        }
    }
}
