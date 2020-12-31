// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.prometheus
{
    #region using Clauses
    using Prometheus;
    #endregion

    /// <summary>
    /// A counter that increments in values and never decreases, only restarting when it is recreated, storing its values using prometheus
    /// </summary>
    public sealed class PrometheusCounter : metrics.ICounter
    {
        #region Variables
        /// <summary>
        /// The Prometheus metric to write to
        /// </summary>
        private readonly Counter _metric;
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="name">The name of the counter</param>
        /// <param name="delayPublish">True if the counter should wait to publish information until it has a value assigned</param>
        /// <param name="help">The help text associated with the counter</param>
        /// <param name="labels">The labels assocaited with the counter</param>
        public PrometheusCounter(string name, string help, bool delayPublish, string[] labels)
        {
            Name = name.Replace('-', '_');
            Help = help;

            if((labels?.Length ?? 0) > 0)
            {
                for(var index = 0; index < labels.Length; index++)
                {
                    labels[index] = labels[index].Replace('-', '_');

                    while(labels[index].Length < 5)
                    {
                        labels[index] += "0";
                    }
                }
            }

            Labels = labels;

            _metric = Metrics.CreateCounter(Name, help, new CounterConfiguration() 
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
            _metric.Inc(value);
        }
        #endregion
    }
}
