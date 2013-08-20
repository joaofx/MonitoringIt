namespace Demo
{
    using System;
    using System.Threading;
    using Example;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting app");

            Monitoring.Initialize("MonitoringIt");

            Console.WriteLine("Press ctrl+c to quit");

            while (true)
            {
                Monitoring.TransactionsPerSecond.Increment();
                Thread.Sleep(200);
            }
        }
    }
}
