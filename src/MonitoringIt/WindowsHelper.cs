namespace MonitoringIt
{
    using System.Runtime.InteropServices;

    public class WindowsHelper
    {
        [DllImport("kernel32.dll")]
        public static extern void QueryPerformanceCounter(ref long ticks);
    }
}
