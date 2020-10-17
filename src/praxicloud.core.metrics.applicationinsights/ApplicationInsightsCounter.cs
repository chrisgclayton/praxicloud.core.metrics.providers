// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights
{
    #region using Clauses
    using Microsoft.ApplicationInsights;
    #endregion

    /// <summary>
    /// A counter that increments in values and never decreases, only restarting when it is recreated, storing its data in Application Insights
    /// </summary>
    public sealed class ApplicationInsightsCounter : ICounter
    {
        #region Variables
        /// <summary>
        /// The metric container in use
        /// </summary>
        private readonly Metric _metric;

        /// <summary>
        /// An instance used to write the metric with required labels
        /// </summary>      
        private readonly DimensionalWriter _writer;

        /// <summary>
        /// The current value of the counter
        /// </summary>
        private double _value = 0;

        /// <summary>
        /// A control used to ensure accurate updates
        /// </summary>
        private readonly object _control = new object();
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="client">The telemetry client being written to</param>
        /// <param name="name">The name of the counter</param>
        /// <param name="help">The help text associated with the counter</param>
        /// <param name="labels">The labels assocaited with the counter</param>
        public ApplicationInsightsCounter(TelemetryClient client, string name, string help, string[] labels)
        {
            Name = name;
            Help = help;
            Labels = labels;
                        
            _metric = client.GetMetric(Name);
            _writer = new DimensionalWriter(_metric, labels);
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
        public void Increment()
        {
            IncrementBy(1.0);
        }

        /// <inheritdoc />
        public void IncrementBy(double value)
        {
            lock(_control)
            {
                _value += value;
                _writer.RecordValue(_value);
            }
        }

        /// <inheritdoc />
        public void SetTo(double value)
        {
            lock (_control)
            {
                _value = value;
                _writer.RecordValue(_value);
            }
        }        
        #endregion
    }
}
