using System.Collections.Generic;
using Newtonsoft.Json;
using TrexExporter.Metadata;

namespace TrexExporter.Models
{
    [AddInstrumentation("dual_stat")]
    public partial class DualStat
    {
        [JsonProperty("accepted_count")]
        [Metric("Counter")]
        public int AcceptedCount { get; set; }

        [JsonProperty("active_pool")]
        public ActivePool ActivePool { get; set; }

        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

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

        [JsonProperty("rejected_count")]
        [Metric("Counter")]
        public int RejectedCount { get; set; }

        [JsonProperty("sharerate")]
        [Metric("Gauge")]
        public double Sharerate { get; set; }

        [JsonProperty("sharerate_average")]
        [Metric("Gauge")]
        public double SharerateAverage { get; set; }

        [JsonProperty("solved_count")]
        [Metric("Counter")]
        public int SolvedCount { get; set; }

        [JsonProperty("watchdog_stat")]
        public WatchdogStat WatchdogStat { get; set; }
    }
}