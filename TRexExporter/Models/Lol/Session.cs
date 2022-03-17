using Newtonsoft.Json;
using TrexExporter.Metadata;

namespace TrexExporter.Models.Lol
{
    [AddInstrumentation("shares")]
    public partial class Session
    {
        [JsonProperty("Startup")]
        public int Startup { get; set; }

        [JsonProperty("Startup_String")]
        public string StartupString { get; set; }

        [JsonProperty("Uptime")]
        [Metric("Counter", metricName: "uptime")]
        public int Uptime { get; set; }

        [JsonProperty("Last_Update")]
        public int LastUpdate { get; set; }

        [JsonProperty("Active_GPUs")]
        public int ActiveGPUs { get; set; }

        [JsonProperty("Performance_Summary")]
        public double PerformanceSummary { get; set; }

        [JsonProperty("Performance_Unit")]
        public string PerformanceUnit { get; set; }

        [JsonProperty("Accepted")]
        public int Accepted { get; set; }

        [JsonProperty("Submitted")]
        public int Submitted { get; set; }

        [JsonProperty("Stale")]
        public int Stale { get; set; }

        [JsonProperty("TotalPower")]
        public double TotalPower { get; set; }
    }
}
