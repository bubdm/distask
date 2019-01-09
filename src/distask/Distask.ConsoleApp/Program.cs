using Distask.TaskDispatchers;
using Distask.Routing;
using System;
using System.Threading;
using System.Timers;
using Distask.TaskDispatchers.Client;

namespace Distask.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var distributor = new TaskDispatcher(new RandomizedRouter()))
            {
                distributor.BrokerClientRegistered += Distributor_BrokerRegistered;
                var timer = new System.Timers.Timer
                {
                    Interval = 300
                };

                timer.Elapsed += (s, e) =>
                 {
                     var managedThreadId = Thread.CurrentThread.ManagedThreadId;
                     try
                     {
                         var response = distributor.DispatchAsync(new RequestMessage("add", new[] { "2", "3" })).Result;
                         if (response.Status == ResponseStatus.Success)
                         {
                             Console.WriteLine($"Thread [{managedThreadId}] Result: {response.Result}");
                         }
                         else
                         {
                             Console.WriteLine(response.ErrorMessage);
                         }
                     }
                     catch (AggregateException ex) when (ex.InnerException != null && ex.InnerException is DistaskException dex)
                     {
                         Console.WriteLine($"Thread [{managedThreadId}] Failed: {dex.Message}");
                     }
                 };
                timer.Start();
                Console.WriteLine("Master started, press ENTER to exit...");
                Console.ReadLine();
                distributor.BrokerClientRegistered -= Distributor_BrokerRegistered;
                timer.Stop();
            }
        }

        private static void Distributor_BrokerRegistered(object sender, BrokerClientRegisteredEventArgs e)
        {
            Console.WriteLine($"New broker registered with the following information: {e}");
        }
    }
}
