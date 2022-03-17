using Newtonsoft.Json;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models.TRex { public partial class Gpu {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
{$"{prefix}_gpus_hashrate", Metrics.CreateGauge($"{prefix}_gpus_hashrate", "hashrate", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_hashrate_day", Metrics.CreateGauge($"{prefix}_gpus_hashrate_day", "hashrate_day", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_hashrate_hour", Metrics.CreateGauge($"{prefix}_gpus_hashrate_hour", "hashrate_hour", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_hashrate_instant", Metrics.CreateGauge($"{prefix}_gpus_hashrate_instant", "hashrate_instant", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_hashrate_minute", Metrics.CreateGauge($"{prefix}_gpus_hashrate_minute", "hashrate_minute", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_cclock", Metrics.CreateGauge($"{prefix}_gpus_cclock", "cclock", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_fan_speed", Metrics.CreateGauge($"{prefix}_gpus_fan_speed", "fan_speed", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_intensity", Metrics.CreateGauge($"{prefix}_gpus_intensity", "intensity", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_lhr_tune", Metrics.CreateGauge($"{prefix}_gpus_lhr_tune", "lhr_tune", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_mclock", Metrics.CreateGauge($"{prefix}_gpus_mclock", "mclock", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_mtweak", Metrics.CreateGauge($"{prefix}_gpus_mtweak", "mtweak", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_power", Metrics.CreateGauge($"{prefix}_gpus_power", "power", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_power_avr", Metrics.CreateGauge($"{prefix}_gpus_power_avr", "power_avr", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_temperature", Metrics.CreateGauge($"{prefix}_gpus_temperature", "temperature", "host", "slot", "algo", "gpu_id", "vendor", "name") },
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, MetricCollection metrics, Gpu data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }
(metrics[$"{prefix}_gpus_hashrate"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Hashrate);
(metrics[$"{prefix}_gpus_hashrate_day"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateDay);
(metrics[$"{prefix}_gpus_hashrate_hour"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateHour);
(metrics[$"{prefix}_gpus_hashrate_instant"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateInstant);
(metrics[$"{prefix}_gpus_hashrate_minute"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateMinute);
(metrics[$"{prefix}_gpus_cclock"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Cclock);
(metrics[$"{prefix}_gpus_fan_speed"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.FanSpeed);
(metrics[$"{prefix}_gpus_intensity"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Intensity);
(metrics[$"{prefix}_gpus_lhr_tune"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.LhrTune);
(metrics[$"{prefix}_gpus_mclock"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Mclock);
(metrics[$"{prefix}_gpus_mtweak"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Mtweak);
(metrics[$"{prefix}_gpus_power"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Power);
(metrics[$"{prefix}_gpus_power_avr"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.PowerAvr);
(metrics[$"{prefix}_gpus_temperature"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Temperature);
}


}}
