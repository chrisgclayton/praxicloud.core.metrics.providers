// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus
{
    #region using Clauses
    using Prometheus;
    using System;
    #endregion

    /// <summary>
    /// A metric that indicates an known event occurred, storing its data in storing its values using prometheus
    /// </summary>
    public sealed class PrometheusGauge : metrics.IGauge
    {
        #region Variables
        /// <summary>
        /// The Prometheus metric to write to
        /// </summary>
        private readonly Gauge _metric;
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="name">The name of the gauge</param>
        /// <param name="delayPublish">True if the gauge should wait to publish information until it has a value assigned</param>
        /// <param name="help">The help text associated with the gauge</param>
        /// <param name="labels">The labels assocaited with the gauge</param>
        public PrometheusGauge(string name, string help, bool delayPublish, string[] labels)
        {
            Name = name;
            Help = help;
            Labels = labels;

            _metric = Metrics.CreateGauge(Name, help, new GaugeConfiguration()
            {
                LabelNames = (labels?.Length ?? 0) < 1 ? null : labels,
                SuppressInitialValue = delayPublish
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
        public void Decrement()
        {
            _metric.Dec();
        }

        /// <inheritdoc />
        public void DecrementBy(double value)
        {
            _metric.Dec(value);
        }

        /// <inheritdoc />
        public void Increment()
        {
            _metric.Inc();
        }

        /// <inheritdoc />
        public void IncrementBy(double value)
        {
            _metric.Inc(value);
        }

        /// <inheritdoc />
        public void SetTo(double value)
        {
            _metric.Set(value);
        }

        /// <inheritdoc />
        public IDisposable TrackExecution()
        {
            return _metric.TrackInProgress();
        }
        #endregion
    }
}
