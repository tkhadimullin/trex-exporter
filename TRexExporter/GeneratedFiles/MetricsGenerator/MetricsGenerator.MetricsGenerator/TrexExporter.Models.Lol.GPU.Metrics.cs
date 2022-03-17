using Newtonsoft.Json;
using System.Collections.Generic;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models.Lol { public partial class GPU {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
{$"{prefix}_gpus_hashrate", Metrics.CreateGauge($"{prefix}_gpus_hashrate", "hashrate", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_power", Metrics.CreateGauge($"{prefix}_gpus_power", "power", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_fan_speed", Metrics.CreateGauge($"{prefix}_gpus_fan_speed", "fan_speed", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_temperature", Metrics.CreateGauge($"{prefix}_gpus_temperature", "temperature", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_accepted_count", Metrics.CreateCounter($"{prefix}_gpus_accepted_count", "accepted_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_invalid_count", Metrics.CreateCounter($"{prefix}_gpus_invalid_count", "invalid_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_gpus_solved_count", Metrics.CreateCounter($"{prefix}_gpus_solved_count", "solved_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, MetricCollection metrics, GPU data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }
(metrics[$"{prefix}_gpus_hashrate"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Performance);
(metrics[$"{prefix}_gpus_power"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.ConsumptionW);
(metrics[$"{prefix}_gpus_fan_speed"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.FanSpeed);
(metrics[$"{prefix}_gpus_temperature"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.TempDegC);
(metrics[$"{prefix}_gpus_accepted_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.SessionAccepted);
(metrics[$"{prefix}_gpus_invalid_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.SessionStale);
(metrics[$"{prefix}_gpus_solved_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.SessionSubmitted);
}


}}
