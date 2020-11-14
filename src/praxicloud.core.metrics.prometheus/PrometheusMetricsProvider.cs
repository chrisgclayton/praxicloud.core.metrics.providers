// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus
{
    #region Using Clauses
    using Prometheus;
    using System;
    #endregion

    /// <summary>
    /// A Prometheus provider for metrics
    /// </summary>
    public sealed class PrometheusMetricsProvider : IMetricProvider, IDisposable
    {
        #region Variables
        /// <summary>
        /// A single instance of the Prometheus metrics server
        /// </summary>
        private readonly IMetricServer _metricServer;

        /// <summary>
        /// The number of metrics in a bucket, use multiples of 500 for optimal performance
        /// </summary>
        private readonly int _bufferSize;

        /// <summary>
        /// The number of buckets to keep before aging out
        /// </summary>
        private readonly int _ageBuckets;

        /// <summary>
        /// The duration in seconds of a pulse summary
        /// </summary>
        private readonly int _pulseDuration;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="hostName">The host name that will server the data</param>
        /// <param name="port">The port that the metrics server willb exposed on</param>
        /// <param name="url">The URL suffix the metrics server will be exposed on (default: metrics/)</param>
        /// <param name="bufferSize">The number of metrics in a bucket, use multiples of 500 for optimal performance</param>
        /// <param name="ageBuckets">The number of buckets to keep before aging out</param>
        /// <param name="pulseDuration">The duration in seconds of a pulse summary</param>
        public PrometheusMetricsProvider(string hostName, int port, string url = "metrics/", int bufferSize = 1500, int ageBuckets = 5, int pulseDuration = 10)
        {
            _bufferSize = bufferSize;
            _ageBuckets = ageBuckets;
            _pulseDuration = pulseDuration;

            if (string.IsNullOrWhiteSpace(hostName))
            {              
                _metricServer = new MetricServer(port, url);
            }
            else
            {
                _metricServer = new MetricServer(hostName, port, url);
            }

            _metricServer.Start();           
        }
        #endregion
        #region Methods
        /// <inheritdoc />
        public metrics.ICounter CreateCounter(string name, string help, bool delayPublish, string[] labels)
        {
            return new PrometheusCounter(name, help, delayPublish, labels);
        }

        /// <inheritdoc />
        public metrics.IGauge CreateGauge(string name, string help, bool delayPublish, string[] labels)
        {
            return new PrometheusGauge(name, help, delayPublish, labels);
        }

        /// <inheritdoc />
        public metrics.IPulse CreatePulse(string name, string help, bool delayPublish, string[] labels)
        {
            return new PrometheusPulse(name, help, _pulseDuration, delayPublish, labels, _bufferSize, _ageBuckets);
        }

        /// <inheritdoc />
        public metrics.ISummary CreateSummary(string name, string help, long duration, bool delayPublish, string[] labels)
        {
            return new PrometheusSummary(name, help, duration, delayPublish, labels, _bufferSize, _ageBuckets);
        }

        public void Dispose()
        {
            _metricServer.Stop();
        }
        #endregion
    }
}
