namespace Demo
{
    using System;
    using System.Reactive.Subjects;
    using System.Threading;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting app");

            Monitoring.Initialize("MonitoringIt");

            Console.WriteLine("Press ctrl+c to quit");

            var counter = 0;
            var items = new Subject<int>();
            items.Subscribe(Process);
            
            while (true)
            {
                items.OnNext(counter++);
                SimulateProcessingTransactions();
            }
        }

        private static void Process(int item)
        {
            Monitoring.TransactionsPerSecond.Increment();
            Monitoring.TransactionsProcessed.Increment();

            //// this monitor was not initialized, but the app still working
            NotInitializedMonitoring.Foo.Increment();
        }

        private static void SimulateProcessingTransactions()
        {
            Thread.Sleep(100);
        }
    }
}
