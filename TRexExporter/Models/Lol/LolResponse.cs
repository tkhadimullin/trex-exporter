using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrexExporter.Models.Lol
{
    public partial class LolResponse
    {
        [JsonProperty("Software")]
        public string Software { get; set; }

        [JsonProperty("Mining")]
        public Mining Mining { get; set; }

        [JsonProperty("Stratum")]
        public Stratum Stratum { get; set; }

        [JsonProperty("Session")]
        public Session Session { get; set; }

        [JsonProperty("GPUs")]
        public List<GPU> GPUs { get; set; }
    }
}
