namespace Demo
{
    using System;
    using System.Threading;

    class Program
    {
        private static int counter;
        private static bool increment;

        static void Main()
        {
            Console.WriteLine("Starting app");

            Monitoring.Initialize("MonitoringIt");

            Console.WriteLine("Press ctrl+c to quit");

            counter = 1;
            increment = true;

            while (true)
            {
                Monitoring.TransactionsPerSecond.Increment();
                Monitoring.TransactionsProcessed.Increment();

                NotInitializedMonitoring.Foo.Increment();

                SimulateProcessingTransactions();
            }
        }

        private static void SimulateProcessingTransactions()
        {
            Thread.Sleep(10 * counter);

            if (increment)
            {
                counter++;
            }
            else
            {
                counter--;
            }

            if (counter > 5)
            {
                increment = false;
            }
            else if (counter == 1)
            {
                increment = true;
            }
        }
    }
}
