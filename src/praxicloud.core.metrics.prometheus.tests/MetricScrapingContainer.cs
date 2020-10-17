// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus.tests
{
    #region Using Clauses
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    #endregion

    /// <summary>
    /// A container for metrics scraped from the Prometheus endpoint
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MetricScrapingContainer
    {
        /// <summary>
        /// The name of the metric
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// The type of the metric that was scraped
        /// </summary>
        public string MetricType { get; set; }

        /// <summary>
        /// The help text associated with the metric
        /// </summary>
        public string MetricHelp { get; set; }

        /// <summary>
        /// Null if the value was not valid or provided, otherwise the value of the counter
        /// </summary>
        public double? MetricValue { get; set; }

        /// <summary>
        /// Parses the scraped text and returns it as a set of metrics
        /// </summary>
        /// <param name="scrapedText">The text to be parsed</param>
        /// <returns>A dictionary of metrics tretrieved keyed by metric name</returns>
        public static Dictionary<string, MetricScrapingContainer> Parse(string scrapedText)
        {
            var results = new Dictionary<string, MetricScrapingContainer>();
            var lines = scrapedText.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            MetricScrapingContainer currentContainer = null;

            foreach (var line in lines)
            {
                var trimmedLine = line.TrimStart();

                if (trimmedLine.StartsWith("# HELP"))
                {
                    if(currentContainer != null)
                    {
                        results.Add(currentContainer.MetricName, currentContainer);
                    }

                    currentContainer = new MetricScrapingContainer();
                    currentContainer.MetricHelp = trimmedLine.Substring(trimmedLine.IndexOf(' ', 2) + 1);
                }
                else if (trimmedLine.StartsWith("# TYPE"))
                {
                    var workingType = trimmedLine.Substring(trimmedLine.IndexOf(' ', 2) + 1);
                    var workingElements = workingType.Split(" ");

                    if (workingElements.Length > 1)
                    {
                        currentContainer.MetricName = workingElements[0];
                        currentContainer.MetricType = workingElements[1];
                    }
                }
                else
                {
                    if (double.TryParse(trimmedLine.Substring(trimmedLine.IndexOf(" ") + 1).Trim(), out var value))
                    {
                        currentContainer.MetricValue = value;
                    }
                }
            }

            if(currentContainer != null && !results.ContainsKey(currentContainer.MetricName))
            {
                results.Add(currentContainer.MetricName, currentContainer);
            }

            return results;
        }     
    }
}
