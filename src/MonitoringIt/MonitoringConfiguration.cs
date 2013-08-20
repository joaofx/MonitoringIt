namespace MonitoringIt
{
    public class MonitoringConfiguration
    {
        static MonitoringConfiguration()
        {
            Name = "MonitoringIt";
        }
        public static string Name
        {
            get;
            private set;
        }

        public static void Init(string categoryName)
        {
            Name = categoryName;
        }
    }
}