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
    public class PulseTests
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
                    var pulse = factory.CreatePulse("Metric1", "Test metric for #1", true, new string[] { "label1", "label2" });

                    for (var index = 0; index < 500; index++)
                    {
                        pulse.Observe();
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
                    Assert.IsTrue((itemList.Max(item => item.Value ?? 0)) >= 1, "Maximum value count not expected");
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
