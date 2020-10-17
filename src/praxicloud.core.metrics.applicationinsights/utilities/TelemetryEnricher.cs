// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.core.metrics.applicationinsights.utilities
{
    #region Using Clauses
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    #endregion

    /// <summary>
    /// A generic telemetry initializer that adds properties to the object if not filtered out
    /// </summary>
    public class TelemetryEnricher : ITelemetryInitializer
    {
        #region Delegates
        /// <summary>
        /// Retrieves a dynamic list of properties to enrich the telemetry item with.
        /// </summary>
        /// <param name="telemetry">The telemetry object that the enrichment properties are being retrieved for</param>
        /// <returns>The enrichment properties to be applied. If default Machine Name key is included the default Machine Key will be excluded</returns>
        public delegate IDictionary<string, string> PropertiesLookup(ITelemetry telemetry);

        /// <summary>
        /// Determines if the telemetry object should be enriched by this enricher
        /// </summary>
        /// <param name="telemetry">The telemetry event being tested for enrichment</param>
        /// <returns>True if the object should be enriched</returns>
        public delegate bool ShouldEnrich(ITelemetry telemetry);
        #endregion
        #region Constants
        /// <summary>
        /// The default Machine Name key that is populated if one is not provided
        /// </summary>
        public const string MachineNameKey = "MachineName";

        /// <summary>
        /// The default process id key that is populated if requested and one is not provided with the same name
        /// </summary>
        public const string ProcessIdKey = "ProcessId";
        #endregion
        #region Variables
        /// <summary>
        /// The enrichment properties that will be added to every telemetry object that is enriched
        /// </summary>
        private readonly ImmutableDictionary<string, string> _enrichmentProperties;

        /// <summary>
        /// The method that is in use to determine if the telemetry object should be enriched
        /// </summary>
        private readonly ShouldEnrich _shouldEnrich;

        /// <summary>
        /// The method that is in use for retrieving a dynamic dictionary to add to the telemetry object
        /// </summary>
        private readonly PropertiesLookup _lookup;

        /// <summary>
        /// True if the process id key should be included in the telemetry enrichment
        /// </summary>
        private readonly bool _includeProcessId;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="enrichmentProperties">The dictionary of properties that are to be used when enriching telemetry objects processed by this initializer</param>
        /// <param name="lookup">A lookup for dynamic properties by telemetry event being processed</param>
        /// <param name="shouldEnrich">A method that returns true if the object should be enriched. By default it enriches all MetricTelemetry and EventTelemetry objects</param>
        /// <param name="includeProcessId">True if the process id should be added to the enrichment properties</param>
        public TelemetryEnricher(Dictionary<string, string> enrichmentProperties = null, PropertiesLookup lookup = null, ShouldEnrich shouldEnrich = null, bool includeProcessId = false)
        {
            _shouldEnrich = shouldEnrich ?? DefaultShouldEnrich;
            _lookup = lookup ?? DefaultPropertiesLookup;
            _enrichmentProperties = enrichmentProperties?.ToImmutableDictionary();
            _includeProcessId = includeProcessId;
        }
        #endregion
        #region ITelemetryInitializer Implementation
        /// <inheritdoc />
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is ISupportProperties telemetryProperties && _shouldEnrich(telemetry))
            {
                if (_enrichmentProperties != null)
                {
                    foreach (var pair in _enrichmentProperties)
                    {
                        telemetryProperties.Properties[pair.Key] = pair.Value;
                    }
                }

                var properties = _lookup(telemetry);

                if (properties != null)
                {
                    foreach (var pair in properties)
                    {
                        telemetryProperties.Properties[pair.Key] = pair.Value;
                    }
                }

                if (!telemetryProperties.Properties.ContainsKey(MachineNameKey)) telemetryProperties.Properties[MachineNameKey] = Environment.MachineName;
                if (_includeProcessId && !telemetryProperties.Properties.ContainsKey(ProcessIdKey)) telemetryProperties.Properties[ProcessIdKey] = Process.GetCurrentProcess().Id.ToString("0");
            }
        }
        #endregion
        #region Default Methods for Delegates if not provided
        /// <summary>
        /// The default check on telemetry to determine if it should enrich it. Defaults to only enriching EventTelemetry and MetricTelemetry types
        /// </summary>
        /// <param name="telemetry">The telemetry object to check for enrichment</param>
        /// <returns>True if the object should be enriched</returns>
        private bool DefaultShouldEnrich(ITelemetry telemetry)
        {
            return telemetry is EventTelemetry || telemetry is MetricTelemetry;
        }

        /// <summary>
        /// The default property lookup for the telemetry item. This implementation always returns a null dictionary
        /// </summary>
        /// <param name="telemetry">The telemetry object that the enrichment properties are being retrieved for</param>
        /// <returns>This implementation always returns a null dictionary</returns>
        private IDictionary<string, string> DefaultPropertiesLookup(ITelemetry telemetry)
        {
            return null;
        }
        #endregion
    }
}

