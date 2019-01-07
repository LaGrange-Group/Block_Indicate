using System;
using System.Threading;

namespace DataCollectionCore
{
    class Program
    {
        static void Main(string[] args)
        {

            Run run = new Run();
            run.RunAsync();
            Console.WriteLine("Finished");
            Console.ReadLine();

        }
    }
}
