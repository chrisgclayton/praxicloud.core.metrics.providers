// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights
{
    #region Using Clauses
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// An extension class for metric factories
    /// </summary>
    public static class ApplicationInsightsMetricsExtensions
    {
        /// <summary>
        /// Adds an Application Insights provider to the factory
        /// </summary>
        /// <param name="factory">The factory to add the application insights provider to</param>
        /// <param name="name">The user friendly and unique name of the provider</param>
        /// <param name="instrumentationKey">The application insights instrumentation key to write to</param>
        /// <param name="enrichmentProperties">The dictionary of properties that are to be used when enriching telemetry objects processed by this initializer</param>
        /// <returns>The metric factory</returns>
        public static IMetricFactory AddApplicationInsights(this IMetricFactory factory, string name, string instrumentationKey, Dictionary<string, string> enrichmentProperties = null)
        {            
            factory.AddProvider(name, new ApplicationInsightsMetricsProvider(instrumentationKey, enrichmentProperties));

            return factory;
        }
    }
}
