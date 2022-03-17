using Newtonsoft.Json;

namespace TrexExporter.Models.TRex
{
    public partial class ByGpu
    {
        [JsonProperty("blocks_found")]
        public int BlocksFound { get; set; }

        [JsonProperty("device_id")]
        public int DeviceId { get; set; }

        [JsonProperty("shares_accepted")]
        public int SharesAccepted { get; set; }

        [JsonProperty("shares_invalid")]
        public int SharesInvalid { get; set; }

        [JsonProperty("shares_rejected")]
        public int SharesRejected { get; set; }
    }
}