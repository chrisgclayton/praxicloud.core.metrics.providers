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
    /// A set of tests to validate the counters
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CounterTests
    {
        #region Simple Counter
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
                    var counter = factory.CreateCounter("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        counter.Increment();
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
                    var counter = factory.CreateCounter("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        counter.IncrementBy(10.0);
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
                    var counter = factory.CreateCounter("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        counter.SetTo(setValue);
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

        /// <summary>
        /// Confirms the values stored are successfully retrieved
        /// </summary>
        [TestMethod]
        public void ValueConfirmation()
        {
            var provider = new ApplicationInsightsMetricsProvider("mykey");

            var counter = provider.CreateCounter("metricName1", "metricHelp", true, new string[] { "label1", "label2" });
            var pulse = provider.CreatePulse("metricName2", "metricHelp", true, new string[] { "label1", "label2" });
            var summary = provider.CreateSummary("metricName3", "metricHelp", 10, true, new string[] { "label1", "label2" });
            var gauge = provider.CreateGauge("metricName4", "metricHelp", true, new string[] { "label1", "label2" });

            Assert.IsTrue(string.Equals(counter.Name, "metricName1", StringComparison.Ordinal), "Counter name not expected");
            Assert.IsTrue(string.Equals(counter.Help, "metricHelp", StringComparison.Ordinal), "Counter help not expected");
            Assert.IsTrue(counter.Labels.Length == 2, "Counter labels not expected length");
            Assert.IsTrue(string.Equals(counter.Labels[0], "label1", StringComparison.Ordinal), "Counter label value 0 not expected");
            Assert.IsTrue(string.Equals(counter.Labels[1], "label2", StringComparison.Ordinal), "Counter label value 1 not expected");

            Assert.IsTrue(string.Equals(pulse.Name, "metricName2", StringComparison.Ordinal), "Pulse name not expected");
            Assert.IsTrue(string.Equals(pulse.Help, "metricHelp", StringComparison.Ordinal), "Pulse help not expected");
            Assert.IsTrue(pulse.Labels.Length == 2, "Pulse labels not expected length");
            Assert.IsTrue(string.Equals(pulse.Labels[0], "label1", StringComparison.Ordinal), "Pulse label value 0 not expected");
            Assert.IsTrue(string.Equals(pulse.Labels[1], "label2", StringComparison.Ordinal), "Pulse label value 1 not expected");

            Assert.IsTrue(string.Equals(summary.Name, "metricName3", StringComparison.Ordinal), "Summary name not expected");
            Assert.IsTrue(string.Equals(summary.Help, "metricHelp", StringComparison.Ordinal), "Summary help not expected");
            Assert.IsTrue(summary.Labels.Length == 2, "Summary labels not expected length");
            Assert.IsTrue(string.Equals(summary.Labels[0], "label1", StringComparison.Ordinal), "Summary label value 0 not expected");
            Assert.IsTrue(string.Equals(summary.Labels[1], "label2", StringComparison.Ordinal), "Summary label value 1 not expected");

            Assert.IsTrue(string.Equals(gauge.Name, "metricName4", StringComparison.Ordinal), "Gauge name not expected");
            Assert.IsTrue(string.Equals(gauge.Help, "metricHelp", StringComparison.Ordinal), "Gauge help not expected");
            Assert.IsTrue(gauge.Labels.Length == 2, "Gauge labels not expected length");
            Assert.IsTrue(string.Equals(gauge.Labels[0], "label1", StringComparison.Ordinal), "Gauge label value 0 not expected");
            Assert.IsTrue(string.Equals(gauge.Labels[1], "label2", StringComparison.Ordinal), "Gauge label value 1 not expected");
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
