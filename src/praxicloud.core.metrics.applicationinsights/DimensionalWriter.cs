// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights
{
    #region Using Clauses
    using Microsoft.ApplicationInsights;
    #endregion

    /// <summary>
    /// A helper utility to share across metric types to write data with the appropriate dimensions
    /// </summary>
    internal sealed class DimensionalWriter
    {
        #region Delegates
        /// <summary>
        /// A delegate used to write the required number of labels for this metric
        /// </summary>
        /// <param name="value">The value to set the metric to</param>
        private delegate void LabelRecorderLong(long value);

        /// <summary>
        /// A delegate used to write the required number of labels for this metric
        /// </summary>
        /// <param name="value">The value to set the metric to</param>
        private delegate void LabelRecorderDouble(double value);
        #endregion
        #region Variables
        /// <summary>
        /// The metric container in use
        /// </summary>
        private readonly Metric _metric;

        /// <summary>
        /// A method to write the metric with required labels
        /// </summary>      
        private readonly LabelRecorderLong _labelRecorderLong;

        /// <summary>
        /// A method to write the metric with required labels
        /// </summary>      
        private readonly LabelRecorderDouble _labelRecorderDouble;

        /// <summary>
        /// The labels the metric has
        /// </summary>
        private readonly string[] _labels;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="metric">The metric the writer is associated with</param>
        /// <param name="labels">The labels that the metric has</param>
        internal DimensionalWriter(Metric metric, string[] labels)
        {
            _metric = metric;
            _labels = labels;

            switch (_labels?.Length ?? 0)
            {
                case 0:
                    _labelRecorderLong = RecordMetric0LabelsLong;
                    _labelRecorderDouble = RecordMetric0LabelsDouble;
                    break;

                case 1:
                    _labelRecorderLong = RecordMetric1LabelsLong;
                    _labelRecorderDouble = RecordMetric1LabelsDouble;
                    break;

                case 2:
                    _labelRecorderLong = RecordMetric2LabelsLong;
                    _labelRecorderDouble = RecordMetric2LabelsDouble;
                    break;

                case 3:
                    _labelRecorderLong = RecordMetric3LabelsLong;
                    _labelRecorderDouble = RecordMetric3LabelsDouble;
                    break;

                case 4:
                    _labelRecorderLong = RecordMetric4LabelsLong;
                    _labelRecorderDouble = RecordMetric4LabelsDouble;
                    break;

                case 5:
                    _labelRecorderLong = RecordMetric5LabelsLong;
                    _labelRecorderDouble = RecordMetric5LabelsDouble;
                    break;

                case 6:
                    _labelRecorderLong = RecordMetric6LabelsLong;
                    _labelRecorderDouble = RecordMetric6LabelsDouble;
                    break;

                case 7:
                    _labelRecorderLong = RecordMetric7LabelsLong;
                    _labelRecorderDouble = RecordMetric7LabelsDouble;
                    break;

                case 8:
                    _labelRecorderLong = RecordMetric8LabelsLong;
                    _labelRecorderDouble = RecordMetric8LabelsDouble;
                    break;

                case 9:
                    _labelRecorderLong = RecordMetric9LabelsLong;
                    _labelRecorderDouble = RecordMetric9LabelsDouble;
                    break;

                default:
                    _labelRecorderLong = RecordMetric10LabelsLong;
                    _labelRecorderDouble = RecordMetric10LabelsDouble;
                    break;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Records the value
        /// </summary>
        /// <param name="value">The value to record</param>
        internal void RecordValue(long value)
        {
            _labelRecorderLong(value);
        }

        /// <summary>
        /// Records the value
        /// </summary>
        /// <param name="value">The value to record</param>
        internal void RecordValue(double value)
        {
            _labelRecorderDouble(value);
        }




        /// <summary>
        /// A metric recorder to write 0 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric0LabelsLong(long value)
        {
            _metric.TrackValue(value);
        }

        /// <summary>
        /// A metric recorder to write 1 dimension
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric1LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0]);
        }

        /// <summary>
        /// A metric recorder to write 2 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric2LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1]);
        }

        /// <summary>
        /// A metric recorder to write 3 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric3LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2]);
        }

        /// <summary>
        /// A metric recorder to write 4 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric4LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3]);
        }

        /// <summary>
        /// A metric recorder to write 5 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric5LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4]);
        }

        /// <summary>
        /// A metric recorder to write 6 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric6LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5]);
        }

        /// <summary>
        /// A metric recorder to write 7 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric7LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6]);
        }

        /// <summary>
        /// A metric recorder to write 8 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric8LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6], _labels[7]);
        }

        /// <summary>
        /// A metric recorder to write 9 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric9LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6], _labels[7], _labels[8]);
        }

        /// <summary>
        /// A metric recorder to write 10 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric10LabelsLong(long value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6], _labels[7], _labels[8], _labels[9]);
        }

        /// <summary>
        /// A metric recorder to write 0 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric0LabelsDouble(double value)
        {
            _metric.TrackValue(value);
        }

        /// <summary>
        /// A metric recorder to write 1 dimension
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric1LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0]);
        }

        /// <summary>
        /// A metric recorder to write 2 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric2LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1]);
        }

        /// <summary>
        /// A metric recorder to write 3 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric3LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2]);
        }

        /// <summary>
        /// A metric recorder to write 4 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric4LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3]);
        }

        /// <summary>
        /// A metric recorder to write 5 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric5LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4]);
        }

        /// <summary>
        /// A metric recorder to write 6 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric6LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5]);
        }

        /// <summary>
        /// A metric recorder to write 7 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric7LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6]);
        }

        /// <summary>
        /// A metric recorder to write 8 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric8LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6], _labels[7]);
        }

        /// <summary>
        /// A metric recorder to write 9 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric9LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6], _labels[7], _labels[8]);
        }

        /// <summary>
        /// A metric recorder to write 10 dimensions
        /// </summary>
        /// <param name="value">The value to record</param>
        private void RecordMetric10LabelsDouble(double value)
        {
            _metric.TrackValue(value, _labels[0], _labels[1], _labels[2], _labels[3], _labels[4], _labels[5], _labels[6], _labels[7], _labels[8], _labels[9]);
        }

        #endregion
    }
}
