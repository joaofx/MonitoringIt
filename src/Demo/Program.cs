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

            Monitoring.Register("MonitoringIt");
            Monitoring.Load("MonitoringIt");

            Console.WriteLine("Press ctrl+c to quit");

            var counter = 0;
            var items = new Subject<int>();
            items.Subscribe(Process);
            
            while (true)
            {
                items.OnNext(counter++);

                if (counter == 1000)
                {
                    Monitoring.TransactionsProcessed.Value(0);
                }
            }
        }

        private static void Process(int item)
        {
            Monitoring.TransactionsPerSecond.Increment();
            Monitoring.TransactionsProcessed.Increment();
            Monitoring.TransactionAvarageTime.Begin();

            //// this monitor was not initialized, but the app still working
            NotInitializedMonitoring.Foo.Increment();

            //// simulate working
            Thread.Sleep(100);

            Monitoring.TransactionAvarageTime.Finish();
        }
    }
}
