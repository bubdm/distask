using Distask.TaskDispatchers;
using System;
using System.Threading;
using System.Timers;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Distask.TaskDispatchers.Routing;
using Distask.TaskDispatchers.Client;

namespace Distask.ConsoleApp
{
    class Program
    {
        private static readonly Random rand = new Random(DateTime.Now.Millisecond);

        static void Main(string[] args)
        {
            using (var distributor = new TaskDispatcher(new RandomizedRouter(), new HealthLevelChecker(HealthLevel.Excellent)))
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
