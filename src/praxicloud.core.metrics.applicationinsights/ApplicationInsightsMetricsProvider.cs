// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights
{
    #region Using Clauses
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using praxicloud.core.metrics.applicationinsights.utilities;
    #endregion

    /// <summary>
    /// An application insights provider for metrics
    /// </summary>
    public sealed class ApplicationInsightsMetricsProvider : IMetricProvider
    {
        #region Variables
        /// <summary>
        /// Application Insights client
        /// </summary>
        private readonly TelemetryClient _client;
        #endregion
        #region Constructors
        /// <summary>
        /// A metrics provider that uses Application Insights as its sink
        /// </summary>
        /// <param name="instrumentationKey">The application insights instrumentation key to write to</param>
        /// <param name="enrichmentProperties">The dictionary of properties that are to be used when enriching telemetry objects processed by this initializer</param>
        public ApplicationInsightsMetricsProvider(string instrumentationKey, Dictionary<string, string> enrichmentProperties = null)
        {
            var configuration = new TelemetryConfiguration
            {
                InstrumentationKey = instrumentationKey
            };

            var enricher = new TelemetryEnricher(enrichmentProperties, null, null, true);
            configuration.TelemetryInitializers.Add(enricher);

            _client = new TelemetryClient(configuration);
        }
        #endregion
        #region Methods
        /// <inheritdoc />
        public ICounter CreateCounter(string name, string help, bool delayPublish, string[] labels)
        {
            return new ApplicationInsightsCounter(_client, name, help, labels);
        }

        /// <inheritdoc />
        public IGauge CreateGauge(string name, string help, bool delayPublish, string[] labels)
        {
            return new ApplicationInsightsGauge(_client, name, help, labels);
        }

        /// <inheritdoc />
        public IPulse CreatePulse(string name, string help, bool delayPublish, string[] labels)
        {
            return new ApplicationInsightsPulse(_client, name, help, labels);
        }

        /// <inheritdoc />
        public ISummary CreateSummary(string name, string help, long duration, bool delayPublish, string[] labels)
        {
            return new ApplicationInsightsSummary(_client, name, help, labels);
        }
        #endregion
    }
}
