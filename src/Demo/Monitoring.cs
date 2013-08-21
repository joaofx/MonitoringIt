namespace Demo
{
    using System.Diagnostics;
    using MonitoringIt;

    public class Monitoring : MonitoringBase<Monitoring, PerformanceCounterType>
    {
        public static Monitoring TransactionsPerSecond =
            new Monitoring("Transactions per second", PerformanceCounterType.RateOfCountsPerSecond32);

        public static Monitoring TransactionsProcessed =
            new Monitoring("Transactions processed", PerformanceCounterType.NumberOfItems32);

        public static Monitoring TransactionAvarageTime =
            new Monitoring("Transaction avarage time", PerformanceCounterType.AverageTimer32);

        public Monitoring(string name, PerformanceCounterType type) : base(name, type)
        {
        }
    }
}
