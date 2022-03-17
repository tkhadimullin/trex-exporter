using System;

namespace TrexExporter.Metadata
{
    internal class MetricAttribute : Attribute
    {
        public MetricAttribute(string type) //Counter, Gauge
        {
        }

        public MetricAttribute(string type, params string[] labels) { }

        public MetricAttribute(string type, string metricName, params string[] labels) { }

        public MetricAttribute(string type, string metricName, string valuePath, params string[] labels) { }
    }
}