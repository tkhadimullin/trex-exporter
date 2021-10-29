using System;

namespace TrexExporter.Metadata
{
    internal class MetricAttribute : Attribute
    {
        public MetricAttribute(string type) //Counter, Gauge
        {
        }

        public MetricAttribute(string type, params string[] labels)
        {
        }
    }
}