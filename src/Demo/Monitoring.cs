namespace Example
{
    using System.Diagnostics;
    using MonitoringIt;

    public class Monitoring : MonitoringBase<Monitoring, PerformanceCounterType>
    {
        public static Monitoring TransactionsPerSecond =
            new Monitoring("Example transactions per second", PerformanceCounterType.RateOfCountsPerSecond32);

        public Monitoring(string name, PerformanceCounterType type) : base(name, type)
        {
        }
    }
}
