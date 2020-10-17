// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights
{
    #region using Clauses
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    #endregion

    /// <summary>
    /// A metric that Summarizes the value over a period of time, storing its data in Application Insights
    /// </summary>
    public sealed class ApplicationInsightsSummary : ISummary
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
        public ApplicationInsightsSummary(TelemetryClient client, string name, string help, string[] labels)
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
        public IDisposable Time()
        {
            return new Tracker(this);
        }

        /// <inheritdoc />
        public void Observe(double value)
        {
            _writer.RecordValue(value);
        }
        #endregion
        #region Execution Tracking Type
        /// <summary>
        /// An type that writes the time from instantiation to disposal in a summary observed value
        /// </summary>
        private sealed class Tracker : IDisposable
        {
            #region Variables
            /// <summary>
            /// The summary to track with
            /// </summary>
            private readonly ISummary _summary;

            /// <summary>
            /// The stopwatch to track with
            /// </summary>
            private readonly Stopwatch _watch;
            #endregion
            #region Constructors
            /// <summary>
            /// Initializes a new instance of the type
            /// </summary>
            /// <param name="summary">The summary to modify</param>
            public Tracker(ISummary summary)
            {
                _summary = summary;

                _watch = Stopwatch.StartNew();
            }
            #endregion
            #region Methods
            /// <inheritdoc />
            public void Dispose()
            {
                _watch.Stop();
                _summary.Observe(_watch.ElapsedMilliseconds);
            }
            #endregion
        }
        #endregion
    }
}
