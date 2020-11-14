// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights
{
    #region using Clauses
    using System;
    using Microsoft.ApplicationInsights;
    #endregion

    /// <summary>
    /// A metric that indicates an known event occurred, storing its data in Application Insights
    /// </summary>
    public sealed class ApplicationInsightsPulse : IPulse
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
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="client">The telemetry client being written to</param>
        /// <param name="name">The name of the pulse</param>
        /// <param name="help">The help text associated with the pulse</param>
        /// <param name="labels">The labels assocaited with the pulse</param>
        public ApplicationInsightsPulse(TelemetryClient client, string name, string help, string[] labels)
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
        public void Observe()
        {
            _writer.RecordValue(1.0);
        }
        #endregion

    }
}
