// Copyright (c) Chris Clayton. All rights reserved.
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
    /// A set of tests to validate the gauges
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GaugeTests
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

                var gauge = factory.CreateGauge("demoGauge", "A test counter", false, null);

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

                                if (metricsScraped.ContainsKey("demoGauge"))
                                {
                                    scraped.Add(metricsScraped["demoGauge"]);
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
                    gauge.Increment();
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 6 results should have been collected in this time (value { results.Count })");
            Assert.IsTrue(scraped.Count > 4, $"At least 6 values for the counter should have been found (value { scraped.Count })");
            Assert.IsTrue((scraped.Max(item => item.MetricValue ?? 0)) >= 400, "Maximum value count not expected");
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

                var gauge = factory.CreateGauge("demoGauge", "A test counter", false, null);

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

                                if (metricsScraped.ContainsKey("demoGauge"))
                                {
                                    scraped.Add(metricsScraped["demoGauge"]);
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
                    gauge.IncrementBy(10.0);
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value {results.Count}))");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value {scraped.Count}))");
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

                var gauge = factory.CreateGauge("demoGauge", "A test counter", false, null);

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

                                if (metricsScraped.ContainsKey("demoGauge"))
                                {
                                    scraped.Add(metricsScraped["demoGauge"]);
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
                    gauge.SetTo(testValue);
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value {results.Count}))");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value {scraped.Count}))");
            Assert.IsFalse(scraped.Any(item => item.MetricValue != testValue && item.MetricValue != 0.0), "Values received that were not expected");
        }


        /// <summary>
        /// Tests to make sure the broker can stand up correctly in a basic mode and data retrieved
        /// </summary>
        [TestMethod]
        public void SimpleIncreaseDecrease()
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

                var gauge = factory.CreateGauge("demoGauge", "A test counter", false, null);

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

                                if (metricsScraped.ContainsKey("demoGauge"))
                                {
                                    scraped.Add(metricsScraped["demoGauge"]);
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
                    gauge.Increment();
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                Task.Delay(1000).GetAwaiter().GetResult();

                for (var index = 0; index < 500; index++)
                {
                    gauge.Decrement();
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                Task.Delay(1000).GetAwaiter().GetResult();

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value {results.Count}))");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value {scraped.Count}))");
            Assert.IsTrue((scraped.Max(item => item.MetricValue ?? 0)) >= 450, "Maximum value count not expected");

        }

        /// <summary>
        /// Tests to make sure the broker can stand up correctly in a basic mode and data retrieved
        /// </summary>
        [TestMethod]
        public void SimpleIncreaseDecreaseBy()
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

                var gauge = factory.CreateGauge("demoGauge", "A test counter", false, null);

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

                                if (metricsScraped.ContainsKey("demoGauge"))
                                {
                                    scraped.Add(metricsScraped["demoGauge"]);
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
                    gauge.IncrementBy(10.0);
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                Task.Delay(1000).GetAwaiter().GetResult();

                for (var index = 0; index < 500; index++)
                {
                    gauge.DecrementBy(10.0);
                    startScraping = true;
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                Task.Delay(1000).GetAwaiter().GetResult();

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value {results.Count}))");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value {scraped.Count}))");
            Assert.IsTrue((scraped.Max(item => item.MetricValue ?? 0)) >= 4500, "Maximum value count not expected");
            Assert.IsTrue((scraped[scraped.Count - 1].MetricValue < 500), "Last value not expected");

        }


        /// <summary>
        /// Tracks 500 operations values
        /// </summary>
        [TestMethod]
        public void SimpleGaugeTracker()
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

                var gauge = factory.CreateGauge("demoGauge", "A test counter", false, null);
                                
                var disposalList = new List<IDisposable>();

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

                                if (metricsScraped.ContainsKey("demoGauge"))
                                {
                                    scraped.Add(metricsScraped["demoGauge"]);
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
                    startScraping = true;
                    disposalList.Add(gauge.TrackExecution());
                    if (index < 499) Task.Delay(10).GetAwaiter().GetResult();
                }

                Task.Delay(1000).GetAwaiter().GetResult();

                foreach (var item in disposalList)
                {
                    item.Dispose();
                }

                Task.Delay(1000).GetAwaiter().GetResult();

                continueScraping = false;
            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 4, $"At least 4 results should have been collected in this time (value {results.Count}))");
            Assert.IsTrue(scraped.Count > 4, $"At least 4 values for the counter should have been found (value {scraped.Count}))");
            Assert.IsTrue((scraped.Max(item => item.MetricValue ?? 0)) >= 450, "Maximum value count not expected");
        }
        #endregion
    }
}
