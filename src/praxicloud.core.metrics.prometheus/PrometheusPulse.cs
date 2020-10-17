// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus
{
    #region using Clauses
    using praxicloud.core.security;
    using Prometheus;
    using System;
    #endregion

    /// <summary>
    /// A metric that indicates an known event occurred, storing its data in prometheus
    /// </summary>
    public sealed class PrometheusPulse : metrics.IPulse
    {
        #region Variables
        /// <summary>
        /// The Prometheus metric to write to
        /// </summary>
        private readonly Summary _metric;
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="name">The name of the counter</param>
        /// <param name="delayPublish">True if the counter should wait to publish information until it has a value assigned</param>
        /// <param name="help">The help text associated with the counter</param>
        /// <param name="duration">The duration in seconds of each bucket (the aggregation is across this bucket)</param>
        /// <param name="labels">The labels assocaited with the counter</param>
        /// <param name="bufferSize">Buffer size limit, use multiples of 500</param>
        /// <param name="ageBuckets">Number of buckets used to expire content</param>
        public PrometheusPulse(string name, string help, long duration, bool delayPublish, string[] labels, int bufferSize = 1500, int ageBuckets = 5)
        {
            Guard.NotLessThan(nameof(duration), duration, 1);

            Name = name;
            Help = help;
            Labels = labels;

            _metric = Metrics.CreateSummary(Name, help, new SummaryConfiguration()
            {
                LabelNames = (labels?.Length ?? 0) < 1 ? null : labels,
                SuppressInitialValue = delayPublish,
                MaxAge = TimeSpan.FromSeconds(duration),
                BufferSize = bufferSize,
                AgeBuckets = ageBuckets
            });
        }
        #endregion
        #region Properties
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Help { get; }

        /// <inheritdoc />
        public string[] Labels { get; }
        #endregion
        #region Methods
        /// <inheritdoc />
        public void Observe()
        {
            _metric.Observe(1.0);
        }
        #endregion
    }
}
