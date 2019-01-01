using Distask.Masters;
using System;
using System.Timers;

namespace Distask.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var master = new Master())
            {
                var timer = new Timer
                {
                    Interval = 1000
                };

                timer.Elapsed += (s, e) =>
                 {
                     var response = master.ExecuteAsync(new RequestMessage("add", new[] { "2", "3" })).Result;
                     if (response.Status == Status.Success)
                     {
                         Console.WriteLine(response.Result);
                     }
                     else
                     {
                         Console.WriteLine(response.ErrorMessage);
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
