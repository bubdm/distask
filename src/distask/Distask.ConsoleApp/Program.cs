using Distask.TaskDispatchers;
using System;
using System.Threading;
using System.Timers;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Routing;
using Distask.TaskDispatchers.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace Distask.ConsoleApp
{
    class Program
    {
        private static readonly Random rand = new Random(DateTime.Now.Millisecond);

        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
            });

            serviceCollection.AddScoped<ITaskDispatcher>(sp => new TaskDispatcher(sp.GetService<ILogger<TaskDispatcher>>()));
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var distributor = serviceProvider.GetService<ITaskDispatcher>();

            using (distributor)
            {
                distributor.BrokerClientRegistered += Distributor_BrokerRegistered;

                do
                {
                    while (!Console.KeyAvailable)
                    {
                        // Thread.Sleep(200);
                        try
                        {
                            var x = rand.Next(100);
                            var y = rand.Next(100);
                            var response = distributor.DispatchAsync(new RequestMessage("add", new[] { x.ToString(), y.ToString() })).Result;
                            if (response.Status == ResponseStatus.Success)
                            {
                                Console.WriteLine($" {x} + {y} = {response.Result}");
                            }
                            else
                            {
                                Console.WriteLine(response.ErrorMessage);
                            }
                        }
                        catch (AggregateException ex) when (ex.InnerException != null && ex.InnerException is DistaskException dex)
                        {
                            Console.WriteLine($"Failed: {dex.Message}");
                        }
                    }

                } while (Console.ReadKey(true).Key != ConsoleKey.Enter);

                //var timer = new System.Timers.Timer
                //{
                //    Interval = 300
                //};

                //timer.Elapsed += (s, e) =>
                // {
                //     var managedThreadId = Thread.CurrentThread.ManagedThreadId;
                //     try
                //     {
                //         var x = rand.Next(100);
                //         var y = rand.Next(100);
                //         var response = distributor.DispatchAsync(new RequestMessage("add", new[] { x.ToString(), y.ToString() })).Result;
                //         if (response.Status == ResponseStatus.Success)
                //         {
                //             Console.WriteLine($"Thread [{managedThreadId}] {x} + {y} = {response.Result}");
                //         }
                //         else
                //         {
                //             Console.WriteLine(response.ErrorMessage);
                //         }
                //     }
                //     catch (AggregateException ex) when (ex.InnerException != null && ex.InnerException is DistaskException dex)
                //     {
                //         Console.WriteLine($"Thread [{managedThreadId}] Failed: {dex.Message}");
                //     }
                // };
                //timer.Start();
                //Console.WriteLine("Master started, press ENTER to exit...");
                //Console.ReadLine();
                //timer.Stop();

                distributor.BrokerClientRegistered -= Distributor_BrokerRegistered;
            }
        }

        private static void Distributor_BrokerRegistered(object sender, BrokerClientRegisteredEventArgs e)
        {
            Console.WriteLine($"New broker registered with the following information: {e}");
        }
    }
}
