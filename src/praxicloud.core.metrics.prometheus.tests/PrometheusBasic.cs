// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus.tests
{
    using Microsoft.Identity.Client;
    #region Using Clauses
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// A set of tests to validate the metrics gauge
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrometheusBasic
    {
        /// <summary>
        /// Tests to make sure the broker can stand up correctly in a basic mode and data retrieved
        /// </summary>
        [TestMethod]
        public void ScrapableBroker()
        {
            var endpoint = @"http://localhost:9610/metrics";
            var client = new HttpClient();
            var results = new List<bool>();
            var scraped = new List<MetricScrapingContainer>();
            
            var continueScraping = true;
            var startScraping = false;
            Task scraper;

            using(var factory = new MetricFactory())
            {
                factory.AddPrometheus("prometheus", 9610);

                var counter = factory.CreateCounter("demoCounter", "A test counter", false, null);

                scraper = Task.Factory.StartNew(() =>
                {
                    while(continueScraping)
                    {
                        if(startScraping)
                        {
                            var response = client.GetAsync(endpoint).GetAwaiter().GetResult();

                            results.Add(response.IsSuccessStatusCode);

                            if(response.IsSuccessStatusCode)
                            {
                                var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                var metricsScraped = MetricScrapingContainer.Parse(responseString);

                                if(metricsScraped.ContainsKey("demoCounter"))
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

                for(var index = 0; index <= 500; index++)
                {
                    counter.Increment();
                    startScraping = true;
                    Thread.Sleep(100);
                }

                continueScraping = false;

            }

            Assert.IsFalse(!results.Any(item => item), "There should be successful readings found");
            Assert.IsTrue(results.Count > 40, "At least 40 results should have been collected in this time");
            Assert.IsTrue(scraped.Count > 40, "At least 40 values for the counter should have been found");
        }
    }
}
