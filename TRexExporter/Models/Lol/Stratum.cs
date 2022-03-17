using Newtonsoft.Json;

namespace TrexExporter.Models.Lol
{
    public class Stratum
    {
        [JsonProperty("Current_Pool")]
        public string CurrentPool { get; set; }

        [JsonProperty("Current_User")]
        public string CurrentUser { get; set; }

        [JsonProperty("Average_Latency")]
        public double AverageLatency { get; set; }
    }
}
