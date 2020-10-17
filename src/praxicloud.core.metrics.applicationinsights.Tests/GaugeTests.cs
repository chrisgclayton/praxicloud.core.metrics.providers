// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights.Tests
{
    #region using Clauses
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// A set of tests to validate the gauges
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GaugeTests
    {
        #region Simple Gauge
        /// <summary>
        /// Counts up to 500
        /// </summary>
        [TestMethod]
        public void SimpleCountIteration()
        {
            var singleValues = new ConcurrentBag<SingleMetricHolder>();

            using (var context = ShimsContext.Create())
            {
                SetupTelemetryClientConstructorFakes();
                ShimMetric fakeMetric = new ShimMetric();

                ShimTelemetryClient.AllInstances.GetMetricString = new FakesDelegates.Func<TelemetryClient, string, Metric>((client, name) =>
                {
                    return fakeMetric;
                });

                ShimMetric.AllInstances.TrackValueDoubleStringString = new FakesDelegates.Func<Metric, double, string, string, bool>((metric, value, label1, label2) =>
                {
                    singleValues.Add(new SingleMetricHolder
                    {
                        Labels = new string[] { label1, label2 },
                        Name = "Metric1",
                        SampleTime = DateTime.UtcNow,
                        UserState = null,
                        Value = value
                    });

                    return true;
                });

                using (var factory = new MetricFactory())
                {
                    factory.AddApplicationInsights("appinsights", "mykey", null);
                    var gauge = factory.CreateGauge("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.Increment();
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();
                }

                var groupings = singleValues.GroupBy(item => item.Name).ToArray();

                Assert.IsTrue(groupings.Length == 1, "Metric count not expected");

                foreach (var item in groupings)
                {
                    var itemList = item.OrderBy(item => item.SampleTime).ToArray();

                    Assert.IsTrue(itemList.Length == 500, "Single value count within groupings not within expected tolerance");
                    Assert.IsTrue((itemList.Max(item => item.Value ?? 0)) >= 500, "Maximum value count not expected");
                }
            }
        }

        /// <summary>
        /// Counts up to 500 and back to 0
        /// </summary>
        [TestMethod]
        public void SimpleIncrementDecrementIteration()
        {
            var singleValues = new ConcurrentBag<SingleMetricHolder>();

            using (var context = ShimsContext.Create())
            {
                SetupTelemetryClientConstructorFakes();
                ShimMetric fakeMetric = new ShimMetric();

                ShimTelemetryClient.AllInstances.GetMetricString = new FakesDelegates.Func<TelemetryClient, string, Metric>((client, name) =>
                {
                    return fakeMetric;
                });

                ShimMetric.AllInstances.TrackValueDoubleStringString = new FakesDelegates.Func<Metric, double, string, string, bool>((metric, value, label1, label2) =>
                {
                    singleValues.Add(new SingleMetricHolder
                    {
                        Labels = new string[] { label1, label2 },
                        Name = "Metric1",
                        SampleTime = DateTime.UtcNow,
                        UserState = null,
                        Value = value
                    });

                    return true;
                });

                using (var factory = new MetricFactory())
                {
                    factory.AddApplicationInsights("appinsights", "mykey", null);
                    var gauge = factory.CreateGauge("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.Increment();
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.Decrement();
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();
                }

                var groupings = singleValues.GroupBy(item => item.Name).ToArray();

                Assert.IsTrue(groupings.Length == 1, "Metric count not expected");

                foreach (var item in groupings)
                {
                    var itemList = item.OrderBy(item => item.SampleTime).ToArray();

                    Assert.IsTrue(itemList.Length == 1000, "Single value count within groupings not within expected tolerance");
                    Assert.IsTrue((itemList.Max(item => item.Value ?? 0)) >= 500, "Maximum value count not expected");
                    Assert.IsTrue((itemList.Where(item => (item.Value ?? 0.0) == 1.0).Count()) == 2, "Number of 1.0 values not expected");
                }
            }
        }


        /// <summary>
        /// Counts up to 5000 by 10
        /// </summary>
        [TestMethod]
        public void SimpleCountByIteration()
        {
            var singleValues = new ConcurrentBag<SingleMetricHolder>();

            using (var context = ShimsContext.Create())
            {
                SetupTelemetryClientConstructorFakes();
                ShimMetric fakeMetric = new ShimMetric();

                ShimTelemetryClient.AllInstances.GetMetricString = new FakesDelegates.Func<TelemetryClient, string, Metric>((client, name) =>
                {
                    return fakeMetric;
                });

                ShimMetric.AllInstances.TrackValueDoubleStringString = new FakesDelegates.Func<Metric, double, string, string, bool>((metric, value, label1, label2) =>
                {
                    singleValues.Add(new SingleMetricHolder
                    {
                        Labels = new string[] { label1, label2 },
                        Name = "Metric1",
                        SampleTime = DateTime.UtcNow,
                        UserState = null,
                        Value = value
                    });

                    return true;
                });

                using (var factory = new MetricFactory())
                {
                    factory.AddApplicationInsights("appinsights", "mykey", null);
                    var gauge = factory.CreateGauge("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.IncrementBy(10.0);
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();
                }

                var groupings = singleValues.GroupBy(item => item.Name).ToArray();

                Assert.IsTrue(groupings.Length == 1, "Metric count not expected");

                foreach (var item in groupings)
                {
                    var itemList = item.OrderBy(item => item.SampleTime).ToArray();

                    Assert.IsTrue(itemList.Length == 500, "Single value count within groupings not within expected tolerance");
                    Assert.IsTrue((itemList.Max(item => item.Value ?? 0)) >= 5000, "Maximum value count not expected");
                }
            }
        }

        /// <summary>
        /// Counts up to 500 and back to 0
        /// </summary>
        [TestMethod]
        public void SimpleIncrementDecrementByIteration()
        {
            var singleValues = new ConcurrentBag<SingleMetricHolder>();

            using (var context = ShimsContext.Create())
            {
                SetupTelemetryClientConstructorFakes();
                ShimMetric fakeMetric = new ShimMetric();

                ShimTelemetryClient.AllInstances.GetMetricString = new FakesDelegates.Func<TelemetryClient, string, Metric>((client, name) =>
                {
                    return fakeMetric;
                });

                ShimMetric.AllInstances.TrackValueDoubleStringString = new FakesDelegates.Func<Metric, double, string, string, bool>((metric, value, label1, label2) =>
                {
                    singleValues.Add(new SingleMetricHolder
                    {
                        Labels = new string[] { label1, label2 },
                        Name = "Metric1",
                        SampleTime = DateTime.UtcNow,
                        UserState = null,
                        Value = value
                    });

                    return true;
                });

                using (var factory = new MetricFactory())
                {
                    factory.AddApplicationInsights("appinsights", "mykey", null);
                    var gauge = factory.CreateGauge("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.IncrementBy(10.0);
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.DecrementBy(10.0);
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();
                }

                var groupings = singleValues.GroupBy(item => item.Name).ToArray();

                Assert.IsTrue(groupings.Length == 1, "Metric count not expected");

                foreach (var item in groupings)
                {
                    var itemList = item.OrderBy(item => item.SampleTime).ToArray();

                    Assert.IsTrue(itemList.Length == 1000, "Single value count within groupings not within expected tolerance");
                    Assert.IsTrue((itemList.Max(item => item.Value ?? 0)) >= 500, "Maximum value count not expected");
                    Assert.IsTrue((itemList.Where(item => (item.Value ?? 0.0) == 10.0).Count()) == 2, "Number of 1.0 values not expected");
                }
            }
        }

        /// <summary>
        /// Sets the value 500 times to a fixed number
        /// </summary>
        [TestMethod]
        public void SimpleSetByIteration()
        {
            var singleValues = new ConcurrentBag<SingleMetricHolder>();
            var setValue = 26.0;

            using (var context = ShimsContext.Create())
            {
                SetupTelemetryClientConstructorFakes();
                ShimMetric fakeMetric = new ShimMetric();

                ShimTelemetryClient.AllInstances.GetMetricString = new FakesDelegates.Func<TelemetryClient, string, Metric>((client, name) =>
                {
                    return fakeMetric;
                });

                ShimMetric.AllInstances.TrackValueDoubleStringString = new FakesDelegates.Func<Metric, double, string, string, bool>((metric, value, label1, label2) =>
                {
                    singleValues.Add(new SingleMetricHolder
                    {
                        Labels = new string[] { label1, label2 },
                        Name = "Metric1",
                        SampleTime = DateTime.UtcNow,
                        UserState = null,
                        Value = value
                    });

                    return true;
                });

                using (var factory = new MetricFactory())
                {
                    factory.AddApplicationInsights("appinsights", "mykey", null);
                    var gauge = factory.CreateGauge("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        gauge.SetTo(setValue);
                        if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                    }

                    Task.Delay(1000).GetAwaiter().GetResult();
                }

                var groupings = singleValues.GroupBy(item => item.Name).ToArray();

                Assert.IsTrue(groupings.Length == 1, "Metric count not expected");

                foreach (var item in groupings)
                {
                    var itemList = item.OrderBy(item => item.SampleTime).ToArray();

                    Assert.IsTrue(itemList.Length == 500, "Single value count within groupings not within expected tolerance");
                    Assert.IsTrue((itemList.Max(item => item.Value ?? 0)) >= setValue, "Maximum value count not expected");
                }
            }
        }
        #endregion
        #region Support Methods
        /// <summary>
        /// Initializes the telemetry client constructors to use fakes
        /// </summary>
        public void SetupTelemetryClientConstructorFakes()
        {
            ShimTelemetryClient.Constructor = new FakesDelegates.Action<Microsoft.ApplicationInsights.TelemetryClient>((client) => { });
            ShimTelemetryClient.ConstructorTelemetryConfiguration = new FakesDelegates.Action<Microsoft.ApplicationInsights.TelemetryClient, Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration>((client, configuration) => { });

        }
        #endregion
    }
}
