MonitoringIt
================================

A lib that helps you write Windows performance counter easily.

If you don't know what Windows performance counter is, check out [this article](http://www.codeproject.com/Articles/8590/An-Introduction-To-Performance-Counters)

Installing
---------------

Install MonitoringIt using NuGet:

```bat
PM> Install-Package MonitoringIt
```

Configuring
---------------

Create a *Monitoring* class inheriting from MonitoringBase:

```csharp
public class Monitoring : MonitoringBase<Monitoring, PerformanceCounterType>
{
    public Monitoring(string name, PerformanceCounterType type) : base(name, type)
    {
    }
}
```

Initializing
---------------

In your application start up code, call <code>Monitoring.Initialize("My App Name");</code> passing your application name.

```csharp
Monitoring.Initialize("MonitoringIt");
```

Requirement
---------------

The <code>Initialize()</code> method will create the performance counter category with the name you passed in parameter.

**To create the category, the application must have administrator previlegies**.

The Initialize returns a object that you can check if a exception was thrown

Creating counters
---------------

To add a performance counter, add a static field in Monitoring class:

```csharp
public class Monitoring : MonitoringBase<Monitoring, PerformanceCounterType>
{
    public static Monitoring TransactionsPerSecond =
        new Monitoring("Transactions per second", PerformanceCounterType.RateOfCountsPerSecond32);

    public Monitoring(string name, PerformanceCounterType type) : base(name, type)
    {
    }
}
```

Incrementing counters
---------------

In your application, use the static field that you've created to increment counters:

```csharp
Monitoring.TransactionsPerSecond.Increment();
```

Demo
---------------

See the demo project in src\Demo

TODO
--------------------------------

* Demo using all metrics
* Show how to configure perfmon
* License
* Automated tests
* Refactoring
* Getting-started
* CI Travis
* Auto push to nuget
