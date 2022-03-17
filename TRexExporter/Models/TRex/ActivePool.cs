// Auto-generated

// https://json2csharp.com/

using Newtonsoft.Json;

namespace TrexExporter.Models.TRex
{
    public partial class ActivePool
    {
        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("last_submit_ts")]
        public int LastSubmitTs { get; set; }

        [JsonProperty("ping")]
        public int Ping { get; set; }

        [JsonProperty("retries")]
        public int Retries { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("worker")]
        public string Worker { get; set; }
    }
}