﻿using Distask.Distributors;
using System;
using System.Threading;
using System.Timers;

namespace Distask.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var distributor = new Distributor())
            {
                var timer = new System.Timers.Timer
                {
                    Interval = 1000
                };

                timer.Elapsed += (s, e) =>
                 {
                     var managedThreadId = Thread.CurrentThread.ManagedThreadId;
                     try
                     {
                         var response = distributor.DistributeAsync(new RequestMessage("add", new[] { "2", "3" })).Result;
                         if (response.Status == Status.Success)
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
                timer.Stop();
            }
        }
    }
}
