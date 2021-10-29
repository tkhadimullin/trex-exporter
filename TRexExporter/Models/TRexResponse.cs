using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Prometheus;
using TrexExporter.Metadata;

namespace TrexExporter.Models
{
    [AddInstrumentation]
    public partial class TRexResponse
    {
        [JsonProperty("accepted_count")]
        [Metric("Counter")]
        public int AcceptedCount { get; set; }

        [JsonProperty("active_pool")]
        public ActivePool ActivePool { get; set; }

        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty("api")]
        public string Api { get; set; }

        [JsonProperty("build_date")]
        public string BuildDate { get; set; }

        [JsonProperty("coin")]
        public string Coin { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("driver")]
        public string Driver { get; set; }

        [JsonProperty("dual_stat")]
        public DualStat DualStat { get; set; }

        [JsonProperty("gpu_total")]
        [Metric("Gauge")]
        public int GpuTotal { get; set; }

        [JsonProperty("gpus")]
        public List<Gpu> Gpus { get; set; }

        [JsonProperty("hashrate")]
        [Metric("Gauge")]
        public int Hashrate { get; set; }

        [JsonProperty("hashrate_day")]
        [Metric("Gauge")]
        public int HashrateDay { get; set; }

        [JsonProperty("hashrate_hour")]
        [Metric("Gauge")]
        public int HashrateHour { get; set; }

        [JsonProperty("hashrate_minute")]
        [Metric("Gauge")]
        public int HashrateMinute { get; set; }

        [JsonProperty("invalid_count")]
        [Metric("Counter")]
        public int InvalidCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("os")]
        public string Os { get; set; }

        [JsonProperty("paused")]
        public bool Paused { get; set; }

        [JsonProperty("rejected_count")]
        [Metric("Counter")]
        public int RejectedCount { get; set; }

        [JsonProperty("revision")]
        public string Revision { get; set; }

        [JsonProperty("sharerate")]
        [Metric("Gauge")]
        public double Sharerate { get; set; }

        [JsonProperty("sharerate_average")]
        [Metric("Gauge")]
        public double SharerateAverage { get; set; }

        [JsonProperty("solved_count")]
        [Metric("Counter")]
        public int SolvedCount { get; set; }

        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("uptime")]
        [Metric("Counter")]
        public int Uptime { get; set; }

        [JsonProperty("validate_shares")]
        public bool ValidateShares { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("watchdog_stat")]
        public WatchdogStat WatchdogStat { get; set; }
    }
}