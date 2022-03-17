using Newtonsoft.Json;

namespace TrexExporter.Models.Lol
{
    public class Mining
    {
        [JsonProperty("Algorithm")]
        public string Algorithm { get; set; }
    }
}
