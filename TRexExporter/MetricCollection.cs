using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Prometheus;

namespace TrexExporter
{
    public class MetricCollection
    {
        public Collector this[string key]
        {
            get { 
                if(Metrics.ContainsKey(key)) return Metrics[key];
                return null;
            }
        }

        public Dictionary<string, Collector> Metrics { get; private set; }
        
        public MetricCollection(IConfiguration configuration)
        {
            Metrics = new Dictionary<string, Collector>();
        }

        public void TryAdd(string key, Collector value)
        {
            if(!Metrics.ContainsKey(key)) Metrics.Add(key, value);
        }
    }
}