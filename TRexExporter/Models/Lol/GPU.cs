using Newtonsoft.Json;
using TrexExporter.Metadata;

namespace TrexExporter.Models.Lol
{
    [AddInstrumentation("gpus")]
    public partial class GPU
    {
        [JsonProperty("Index")]
        public int Index { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Performance")]
        [Metric("Gauge", metricName: "hashrate", labels: new[] { "gpu_id", "vendor", "name" })]
        public double Performance { get; set; }

        [JsonProperty("Consumption (W)")]
        [Metric("Gauge", metricName: "power", labels: new[] { "gpu_id", "vendor", "name" })]
        public double ConsumptionW { get; set; }

        [JsonProperty("Fan Speed (%)")]
        [Metric("Gauge", metricName: "fan_speed", labels: new[] { "gpu_id", "vendor", "name" })]
        public int FanSpeed { get; set; }

        [JsonProperty("Temp (deg C)")]
        [Metric("Gauge", metricName: "temperature", labels: new[] { "gpu_id", "vendor", "name" })]
        public int TempDegC { get; set; }

        [JsonProperty("Mem Temp (deg C)")]
        public int MemTempDegC { get; set; }

        [JsonProperty("Session_Accepted")]
        [Metric("Counter", metricName: "accepted_count", labels: new[] { "gpu_id", "vendor", "name" })]
        public int SessionAccepted { get; set; }

        [JsonProperty("Session_Stale")]
        [Metric("Counter", metricName: "invalid_count", labels: new[] { "gpu_id", "vendor", "name" })]
        public int SessionStale { get; set; }

        [JsonProperty("Session_Submitted")]
        [Metric("Counter", metricName: "solved_count", labels: new[] { "gpu_id", "vendor", "name" })]
        public int SessionSubmitted { get; set; }

        [JsonProperty("Session_HWErr")]
        public int SessionHWErr { get; set; }

        [JsonProperty("Session_BestShare")]
        public long SessionBestShare { get; set; }

        [JsonProperty("PCIE_Address")]
        public string PCIEAddress { get; set; }
    }
}
