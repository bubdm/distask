using Distask.Masters;
using System;

namespace Distask.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var master = new Master())
            {
                Console.WriteLine("Master started, press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
