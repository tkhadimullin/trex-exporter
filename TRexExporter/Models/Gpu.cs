using Newtonsoft.Json;
using TrexExporter.Metadata;

namespace TrexExporter.Models
{
    [AddInstrumentation("gpus")]
    public partial class Gpu
    {
        [JsonProperty("device_id")]
        public int DeviceId { get; set; }

        [JsonProperty("hashrate")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int Hashrate { get; set; }

        [JsonProperty("hashrate_day")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int HashrateDay { get; set; }

        [JsonProperty("hashrate_hour")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int HashrateHour { get; set; }

        [JsonProperty("hashrate_instant")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int HashrateInstant { get; set; }

        [JsonProperty("hashrate_minute")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int HashrateMinute { get; set; }

        [JsonProperty("shares")]
        public Shares Shares { get; set; }

        [JsonProperty("cclock")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int Cclock { get; set; }

        [JsonProperty("dag_build_mode")]
        public int DagBuildMode { get; set; }

        [JsonProperty("efficiency")]
        public string Efficiency { get; set; }

        [JsonProperty("fan_speed")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int FanSpeed { get; set; }

        [JsonProperty("gpu_id")]
        public int GpuId { get; set; }

        [JsonProperty("gpu_user_id")]
        public int GpuUserId { get; set; }

        [JsonProperty("intensity")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public double Intensity { get; set; }

        [JsonProperty("lhr_tune")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public double LhrTune { get; set; }

        [JsonProperty("low_load")]
        public bool LowLoad { get; set; }

        [JsonProperty("mclock")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int Mclock { get; set; }

        [JsonProperty("mtweak")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int Mtweak { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("paused")]
        public bool Paused { get; set; }

        [JsonProperty("pci_bus")]
        public int PciBus { get; set; }

        [JsonProperty("pci_domain")]
        public int PciDomain { get; set; }

        [JsonProperty("pci_id")]
        public int PciId { get; set; }

        [JsonProperty("potentially_unstable")]
        public bool PotentiallyUnstable { get; set; }

        [JsonProperty("power")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int Power { get; set; }

        [JsonProperty("power_avr")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int PowerAvr { get; set; }

        [JsonProperty("temperature")]
        [Metric("Gauge", "gpu_id", "vendor", "name")]
        public int Temperature { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }
    }
}