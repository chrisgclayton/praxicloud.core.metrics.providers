# PraxiCloud Core Metrics Providers
PraxiCloud Libraries are a set of common utilities and tools for general software development that simplify common development efforts for software development. The core metrics library contains a basic provider based metrics framework modeled with a factory architecture. The providers library includes additional common providers for use with the framework. Current providers include Application Insights and Prometheus.



# Installing via NuGet

**Application Insights Provider**:

​    Install-Package PraxiCloud-Core-Metrics-ApplicationInsights

**Prometheus Provider**:

   Install-Package PraxiCloud-Core-Metrics-Prometheus



# Application Insights



## Key Types and Interfaces

|Class| Description | Notes |
| ------------- | ------------- | ------------- |
|**ApplicationInsightsMetricsProvider**|A metrics provider that writes metric values to Application Insights based on the instrumentation key provided. The adding of the provider is the only portion that varies from standard use of the metrics framework.|  |
|**ApplicationInsightsMetricsExtensions**|Provides a set of extension methods to add the provider to the metric factory.|  |

## Sample Usage

### Add Application Insights Provider to the Metric Factory 

```csharp
using (var factory = new MetricFactory())
{
    factory.AddProvider("appi", new ApplicationInsightsMetricsProvider(instrumentationKey));
   
    var summary = factory.CreateSummary("Metric1", "Test metric for #1", 5, true, new string[] { "label1", "label2" });

    for (var index = 0; index < 1500; index++)
    {
        summary.Observe(_doubleValues[index % _doubleValues.Length]);
        if (index < 1499) Task.Delay(10).GetAwaiter().GetResult();
    }
}
```

### Add Application Insights Provider to the Metric Factory with Extensions

```csharp
using (var factory = new MetricFactory())
{
    factory.AddApplicationInsights("appi", instrumentationKey);
   
    var summary = factory.CreateSummary("Metric1", "Test metric for #1", 5, true, new string[] { "label1", "label2" });

    for (var index = 0; index < 1500; index++)
    {
        summary.Observe(_doubleValues[index % _doubleValues.Length]);
        if (index < 1499) Task.Delay(10).GetAwaiter().GetResult();
    }
}
```

### Sample Application Insights Query

```kql
customMetrics 
| where timestamp >= ago(24h) and name == "mymetricname" 
| project timestamp, name, appName, value, valueCount, valueMin, valueMax, valueStdDev, customDimensions.MachineName 
| order by timestamp desc 
```

## Additional Information

For additional information the Visual Studio generated documentation found [here](./documents/praxicloud.core.metrics.applicationinsights/praxicloud.core.metrics.applicationinsights.xml), can be viewed using your favorite documentation viewer.




# Prometheus



## Key Types and Interfaces

|Class| Description | Notes |
| ------------- | ------------- | ------------- |
|**PrometheusMetricsProvider**|A metrics provider that exposes a Prometheus endpoint for scraping that the metrics framework writes metric values to. The adding of the provider is the only portion that varies from standard use of the metrics framework.|  |
|**PrometheusMetricsExtensions**|Provides a set of extension methods to add the provider to the metric factory.|  |

## Sample Usage

### Add Prometheus Provider to the Metric Factory (http://localhost:9600)

```csharp
using (var factory = new MetricFactory())
{
    factory.AddProvider("prom", new PrometheusMetricsProvider("localhost", 9600));
   
    var summary = factory.CreateSummary("Metric1", "Test metric for #1", 5, true, new string[] { "label1", "label2" });

    for (var index = 0; index < 1500; index++)
    {
        summary.Observe(_doubleValues[index % _doubleValues.Length]);
        if (index < 1499) Task.Delay(10).GetAwaiter().GetResult();
    }
}
```

### Add Prometheus Provider to the Metric Factory with Extensions (http://localhost:9600)

```csharp
using (var factory = new MetricFactory())
{
    factory.AddPrometheus("appi", "localhost", 9600);
   
    var summary = factory.CreateSummary("Metric1", "Test metric for #1", 5, true, new string[] { "label1", "label2" });

    for (var index = 0; index < 1500; index++)
    {
        summary.Observe(_doubleValues[index % _doubleValues.Length]);
        if (index < 1499) Task.Delay(10).GetAwaiter().GetResult();
    }
}
```

## Additional Information

For additional information the Visual Studio generated documentation found [here](./documents/praxicloud.core.metrics.prometheus/praxicloud.core.metrics.prometheus.xml), can be viewed using your favorite documentation viewer.

