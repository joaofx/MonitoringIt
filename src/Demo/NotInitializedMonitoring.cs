namespace Demo
{
    using System.Diagnostics;
    using MonitoringIt;

    public class NotInitializedMonitoring : MonitoringBase<NotInitializedMonitoring, PerformanceCounterType>
    {
        public static Monitoring Foo =
            new Monitoring("Foo", PerformanceCounterType.RateOfCountsPerSecond32);

        public NotInitializedMonitoring(string name, PerformanceCounterType type)
            : base(name, type)
        {
        }
    }
}
