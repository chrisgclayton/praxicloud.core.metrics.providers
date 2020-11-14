// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus.tests
{
    #region Using Clauses
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
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
        /// Tests to make sure the broker can stand up correctly in a basic mode and data retrieved
        /// </summary>
        [TestMethod]
        public void SimpleCountIteration()
        {
            var endpoint = @"http://localhost:9610/metrics";
            var client = new HttpClient();
            var results = new List<bool>();
            var scraped = new List<MetricScrapingContainer>();

            var continueScraping = true;
            var startScraping = false;
            Task scraper;

            using (var factory = new MetricFactory())
            {
                factory.AddPrometheus("prometheus", 9610);

                var counter = factory.CreateCounter("demoCounter", "A test counter", false, null);

                scraper = Task.Factory.StartNew(() =>
                {
                    while (continueScraping)
                    {
                        if (startScraping)
                        {
                            var response = client.GetAsync(endpoint).GetAwaiter().GetResult();

                            results.Add(response.IsSuccessStatusCode);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                var metricsScraped = MetricScrapingContainer.Parse(responseString);

                                if (metricsScraped.ContainsKey("demoCounter"))
                                {
                                    scraped.Add(metricsScraped["demoCounter"]);
                                }
                            }

                            Task.Delay(1000).GetAwaiter().GetResult();
                        }
                        else
                        {
                            Task.Delay(10).GetAwaiter().GetResult();
                        }
                    }
                });

                for (var index = 0; index < 500; index++)
                {
                    counter.Increment();
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value {results.Count}))");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value {scraped.Count}))");
//            Assert.IsTrue((scraped.Max(item => item.MetricValue ?? 0)) >= 450, "Maximum value count not expected");
        }

        /// <summary>
        /// Counts to 5000 by 10s
        /// </summary>
        [TestMethod]
        public void SimpleCountIterationBy()
        {
            var endpoint = @"http://localhost:9610/metrics";
            var client = new HttpClient();
            var results = new List<bool>();
            var scraped = new List<MetricScrapingContainer>();

            var continueScraping = true;
            var startScraping = false;
            Task scraper;

            using (var factory = new MetricFactory())
            {
                factory.AddPrometheus("prometheus", 9610);

                var counter = factory.CreateCounter("demoCounter", "A test counter", false, null);

                scraper = Task.Factory.StartNew(() =>
                {
                    while (continueScraping)
                    {
                        if (startScraping)
                        {
                            var response = client.GetAsync(endpoint).GetAwaiter().GetResult();

                            results.Add(response.IsSuccessStatusCode);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                var metricsScraped = MetricScrapingContainer.Parse(responseString);

                                if (metricsScraped.ContainsKey("demoCounter"))
                                {
                                    scraped.Add(metricsScraped["demoCounter"]);
                                }
                            }

                            Task.Delay(1000).GetAwaiter().GetResult();
                        }
                        else
                        {
                            Task.Delay(10).GetAwaiter().GetResult();
                        }
                    }
                });

                for (var index = 0; index < 500; index++)
                {
                    counter.IncrementBy(10.0);
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time ({ results.Count })");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found ({ scraped.Count })");
            Assert.IsTrue((scraped.Max(item => item.MetricValue ?? 0)) >= 4500, "Maximum value count not expected");
        }

        /// <summary>
        /// Counts to 5000 by 10s
        /// </summary>
        [TestMethod]
        public void SimpleCountSet()
        {
            var endpoint = @"http://localhost:9610/metrics";
            var client = new HttpClient();
            var results = new List<bool>();
            var scraped = new List<MetricScrapingContainer>();
            var testValue = 123.5678;
            var continueScraping = true;
            var startScraping = false;
            Task scraper;

            using (var factory = new MetricFactory())
            {
                factory.AddPrometheus("prometheus", 9610);

                var counter = factory.CreateCounter("demoCounter", "A test counter", false, null);

                scraper = Task.Factory.StartNew(() =>
                {
                    while (continueScraping)
                    {
                        if (startScraping)
                        {
                            var response = client.GetAsync(endpoint).GetAwaiter().GetResult();

                            results.Add(response.IsSuccessStatusCode);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                var metricsScraped = MetricScrapingContainer.Parse(responseString);

                                if (metricsScraped.ContainsKey("demoCounter"))
                                {
                                    scraped.Add(metricsScraped["demoCounter"]);
                                }
                            }

                            Task.Delay(1000).GetAwaiter().GetResult();
                        }
                        else
                        {
                            Task.Delay(10).GetAwaiter().GetResult();
                        }
                    }
                });

                for (var index = 0; index < 500; index++)
                {
                    counter.SetTo(testValue);
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value { results.Count })");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value { scraped.Count })");
        }




        /// <summary>
        /// Counts up to 500
        /// </summary>
        [TestMethod]
        public void ValueConfirmation()
        {
            var provider = new PrometheusMetricsProvider("localhost", 9610);
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
    }
}
