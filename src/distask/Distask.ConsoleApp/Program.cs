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
using Distask.TaskDispatchers.Config;

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

            var taskDispatcherConfig = new TaskDispatcherConfiguration(new RecyclingConfiguration("0:0:5"), BrokerClientConfiguration.Default);
            var serviceProvider = serviceCollection
                .AddScoped<IRouter, RoundRobinRouter>()
                .AddScoped(s => taskDispatcherConfig)
                .AddScoped<IAvailabilityChecker>(s => new HealthLevelChecker(s.GetService<ILogger<HealthLevelChecker>>(), BrokerClientHealthLevel.Excellent))
                .AddScoped<ITaskDispatcher, TaskDispatcher>()
                .BuildServiceProvider();

            var distributor = serviceProvider.GetService<ITaskDispatcher>();

            using (distributor)
            {
                distributor.BrokerClientRegistered += Distributor_BrokerClientRegistered;
                distributor.BrokerClientRecycled += Distributor_BrokerClientRecycled;
                distributor.BrokerClientDisposed += Distributor_BrokerClientDisposed;
                distributor.RecyclingStarted += Distributor_RecyclingStarted;
                distributor.RecyclingCompleted += Distributor_RecyclingCompleted;


                //Console.Write("Press ENTER to continue...");
                //Console.ReadLine();

                //var result = distributor.DispatchAsync("say-hello").Result;

                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(1000);
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
                            // Console.WriteLine($"Failed: {dex.Message}");
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

                distributor.BrokerClientRegistered -= Distributor_BrokerClientRegistered;
                distributor.BrokerClientRecycled -= Distributor_BrokerClientRecycled;
                distributor.RecyclingStarted -= Distributor_RecyclingStarted;
                distributor.RecyclingCompleted -= Distributor_RecyclingCompleted;
            }
        }

        private static void Distributor_BrokerClientDisposed(object sender, BrokerClientDisposedEventArgs e)
        {
            Console.WriteLine($"Broker client has been disposed. {e}");
        }

        private static void Distributor_RecyclingCompleted(object sender, RecyclingEventArgs e)
        {
            Console.WriteLine("Recycling started.");
        }

        private static void Distributor_RecyclingStarted(object sender, RecyclingEventArgs e)
        {
            Console.WriteLine("Recycling completed.");
        }

        private static void Distributor_BrokerClientRecycled(object sender, BrokerClientRecycledEventArgs e)
        {
            Console.WriteLine($"Broker client has been recycled: {e}");
        }

        private static void Distributor_BrokerClientRegistered(object sender, BrokerClientRegisteredEventArgs e)
        {
            Console.WriteLine($"New broker registered with the following information: {e}");
        }
    }
}
