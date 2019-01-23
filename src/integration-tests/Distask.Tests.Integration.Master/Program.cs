using System;
using System.Threading.Tasks;

namespace Distask.Tests.Integration.Master
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var testRunner = new IntegrationTestRunner();
            await testRunner.RunAsync(args);
        }
    }
}
