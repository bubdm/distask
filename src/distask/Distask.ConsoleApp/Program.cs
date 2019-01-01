using Distask.Distributors;
using System;
using System.Timers;

namespace Distask.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var distributor = new Distributor())
            {
                var timer = new Timer
                {
                    Interval = 1000
                };

                timer.Elapsed += (s, e) =>
                 {
                     try
                     {
                         var response = distributor.DistributeAsync(new RequestMessage("add", new[] { "2", "3" })).Result;
                         if (response.Status == Status.Success)
                         {
                             Console.WriteLine(response.Result);
                         }
                         else
                         {
                             Console.WriteLine(response.ErrorMessage);
                         }
                     }
                     catch(DistaskException dex)
                     {
                         Console.WriteLine(dex.Message);
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
