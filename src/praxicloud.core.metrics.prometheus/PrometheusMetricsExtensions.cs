// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus
{
    #region Using Clauses
    using System.Security.Cryptography.X509Certificates;
    #endregion

    /// <summary>
    /// An extension class for metric factories
    /// </summary>
    public static class PrometheusMetricsExtensions
    {
        /// <summary>
        /// Adds a Prometheus provider to the factory
        /// </summary>
        /// <param name="factory">The factory to add the application insights provider to</param>
        /// <param name="name">The user friendly and unique name of the provider</param>
        /// <param name="hostName">The host name that will server the data</param>
        /// <param name="port">The port that the metrics server willb exposed on</param>
        /// <param name="url">The URL suffix the metrics server will be exposed on (default: metrics/)</param>
        /// <param name="bufferSize">The number of metrics in a bucket, use multiples of 500 for optimal performance</param>
        /// <param name="ageBuckets">The number of buckets to keep before aging out</param>
        /// <param name="pulseDuration">The duration in seconds of a pulse summary</param>
        public static IMetricFactory AddPrometheus(this IMetricFactory factory, string name, int port, string hostName = null, string url = "metrics/", int bufferSize = 1500, int ageBuckets = 5, int pulseDuration = 10)
        {
            factory.AddProvider(name, new PrometheusMetricsProvider(hostName, port, url, bufferSize, ageBuckets, pulseDuration));

            return factory;
        }
    }
}
