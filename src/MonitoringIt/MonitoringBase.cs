namespace MonitoringIt
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    public abstract class MonitoringBase<TMonitoring, TType>
        where TMonitoring : MonitoringBase<TMonitoring, TType>
    {
        private static readonly Lazy<TMonitoring[]> enumerations = new Lazy<TMonitoring[]>(GetEnumerations);
        private static bool initialized;
        private PerformanceCounter counter;
        private PerformanceCounter baseCounter;
        private long startTime;

        protected MonitoringBase(string name, PerformanceCounterType type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name
        {
            get;
            private set;
        }

        public PerformanceCounterType Type
        {
            get;
            private set;
        }

        public static TMonitoring[] GetAll()
        {
            return enumerations.Value;
        }

        public static InitializeResult Initialize(string categoryName)
        {
            if (initialized == false)
            {
                var collection = new CounterCreationDataCollection();
                var monitors = GetAll();
                
                foreach (var monitor in monitors)
                {
                    AddInCollection(monitor, collection);
                }

                try
                {
                    CreateCategory(collection, categoryName);
                }
                catch(Exception exception)
                {
                    return new InitializeResult(exception);            
                }
                finally
                {
                    foreach (var monitor in monitors)
                    {
                        monitor.InitializeCounter(categoryName);
                    }

                    initialized = true;    
                }
            }

            return new InitializeResult();
        }

        public void Increment()
        {
            if (this.counter != null) this.counter.Increment();
        }

        public void IncrementBy(int value)
        {
            if (this.counter != null) this.counter.IncrementBy(value);
        }

        public void Decrement()
        {
            if (this.counter != null) this.counter.Decrement();
        }

        public void Begin()
        {
            if (this.counter != null) WindowsHelper.QueryPerformanceCounter(ref this.startTime);
        }

        public void Finish()
        {
            if (this.counter == null) return;

            long endTime = 0;
            WindowsHelper.QueryPerformanceCounter(ref endTime);

            if (this.baseCounter != null)
            {
                this.counter.IncrementBy(endTime - this.startTime);
                this.baseCounter.Increment();
            }
        }

        public void InitializeCounter(string categoryName)
        {
            this.counter = this.CreatePerformanceCounter(categoryName, this.Name);

            if (this.Type == PerformanceCounterType.AverageTimer32)
            {
                this.baseCounter = this.CreatePerformanceCounter(categoryName, GetBaseName(this.Name));
            }
        }

        private static TMonitoring[] GetEnumerations()
        {
            Type enumerationType = typeof(TMonitoring);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null)).Cast<TMonitoring>().ToArray();
        }

        private static string GetBaseName(string name)
        {
            return name + " base";
        }

        private static void AddInCollection(TMonitoring monitor, CounterCreationDataCollection collection)
        {
            collection.Add(new CounterCreationData
            {
                CounterType = monitor.Type,
                CounterName = monitor.Name
            });

            if (monitor.Type == PerformanceCounterType.AverageTimer32)
            {
                collection.Add(new CounterCreationData
                {
                    CounterType = PerformanceCounterType.AverageBase,
                    CounterName = GetBaseName(monitor.Name)
                });
            }
        }

        private PerformanceCounter CreatePerformanceCounter(string categoryName, string name)
        {
            return new PerformanceCounter(categoryName, name, false);
        }

        private static void CreateCategory(CounterCreationDataCollection collection, string categoryName)
        {
            if (PerformanceCounterCategory.Exists(categoryName))
            {
                PerformanceCounterCategory.Delete(categoryName);
            }

            PerformanceCounterCategory.Create(
                categoryName,
                string.Empty,
                PerformanceCounterCategoryType.SingleInstance,
                collection);
        }
    }
}