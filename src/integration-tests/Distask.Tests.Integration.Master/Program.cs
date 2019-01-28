using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace Distask.Tests.Integration.Master
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Boost the Serilog infrustructure.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                var testRunner = new IntegrationTestRunner();
                await testRunner.RunAsync(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
