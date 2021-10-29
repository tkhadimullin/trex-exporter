using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrexExporter.Models
{
    public partial class WatchdogStat
    {
        [JsonProperty("by_gpu")]
        public List<ByGpu> ByGpu { get; set; }

        [JsonProperty("total_blocks_found")]
        public int TotalBlocksFound { get; set; }

        [JsonProperty("total_shares_accepted")]
        public int TotalSharesAccepted { get; set; }

        [JsonProperty("total_shares_invalid")]
        public int TotalSharesInvalid { get; set; }

        [JsonProperty("total_shares_rejected")]
        public int TotalSharesRejected { get; set; }

        [JsonProperty("built_in")]
        public bool BuiltIn { get; set; }
        
        [JsonProperty("startup_ts")]
        public int StartupTs { get; set; }

        [JsonProperty("total_restarts")]
        public int TotalRestarts { get; set; }

        [JsonProperty("uptime")]
        public int Uptime { get; set; }

        [JsonProperty("wd_version")]
        public string WdVersion { get; set; }
    }
}