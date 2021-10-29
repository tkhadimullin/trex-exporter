using Newtonsoft.Json;
using TrexExporter.Metadata;

namespace TrexExporter.Models
{
    [AddInstrumentation("shares")]
    public partial class Shares
    {
        [JsonProperty("accepted_count")]
        [Metric("Counter", "gpu_id", "vendor", "name")]
        public int AcceptedCount { get; set; }

        [JsonProperty("invalid_count")]
        [Metric("Counter", "gpu_id", "vendor", "name")]
        public int InvalidCount { get; set; }

        [JsonProperty("last_share_diff")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public double LastShareDiff { get; set; }

        [JsonProperty("last_share_submit_ts")]
        [Metric("Counter", "gpu_id", "vendor", "name")]
        public int LastShareSubmitTs { get; set; }
        

        [JsonProperty("max_share_diff")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public double MaxShareDiff { get; set; }
        

        [JsonProperty("max_share_submit_ts")]
        [Metric("Counter", "gpu_id", "vendor", "name")]
        public int MaxShareSubmitTs { get; set; }

        [JsonProperty("rejected_count")]
        [Metric("Counter", "gpu_id", "vendor", "name")]
        public int RejectedCount { get; set; }

        [JsonProperty("solved_count")]
        [Metric("Counter", "gpu_id", "vendor", "name")]
        public int SolvedCount { get; set; }
    }
}