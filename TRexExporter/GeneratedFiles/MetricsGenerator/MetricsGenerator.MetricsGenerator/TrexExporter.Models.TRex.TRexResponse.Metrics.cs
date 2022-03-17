using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Prometheus;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models.TRex { public partial class TRexResponse {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
{$"{prefix}_accepted_count", Metrics.CreateCounter($"{prefix}_accepted_count", "accepted_count", "host", "slot", "algo") },
{$"{prefix}_gpu_total", Metrics.CreateGauge($"{prefix}_gpu_total", "gpu_total", "host", "slot", "algo") },
{$"{prefix}_hashrate", Metrics.CreateGauge($"{prefix}_hashrate", "hashrate", "host", "slot", "algo") },
{$"{prefix}_hashrate_day", Metrics.CreateGauge($"{prefix}_hashrate_day", "hashrate_day", "host", "slot", "algo") },
{$"{prefix}_hashrate_hour", Metrics.CreateGauge($"{prefix}_hashrate_hour", "hashrate_hour", "host", "slot", "algo") },
{$"{prefix}_hashrate_minute", Metrics.CreateGauge($"{prefix}_hashrate_minute", "hashrate_minute", "host", "slot", "algo") },
{$"{prefix}_invalid_count", Metrics.CreateCounter($"{prefix}_invalid_count", "invalid_count", "host", "slot", "algo") },
{$"{prefix}_rejected_count", Metrics.CreateCounter($"{prefix}_rejected_count", "rejected_count", "host", "slot", "algo") },
{$"{prefix}_sharerate", Metrics.CreateGauge($"{prefix}_sharerate", "sharerate", "host", "slot", "algo") },
{$"{prefix}_sharerate_average", Metrics.CreateGauge($"{prefix}_sharerate_average", "sharerate_average", "host", "slot", "algo") },
{$"{prefix}_solved_count", Metrics.CreateCounter($"{prefix}_solved_count", "solved_count", "host", "slot", "algo") },
{$"{prefix}_uptime", Metrics.CreateCounter($"{prefix}_uptime", "uptime", "host", "slot", "algo") },
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, MetricCollection metrics, TRexResponse data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }
(metrics[$"{prefix}_accepted_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.AcceptedCount);
(metrics[$"{prefix}_gpu_total"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.GpuTotal);
(metrics[$"{prefix}_hashrate"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Hashrate);
(metrics[$"{prefix}_hashrate_day"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateDay);
(metrics[$"{prefix}_hashrate_hour"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateHour);
(metrics[$"{prefix}_hashrate_minute"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateMinute);
(metrics[$"{prefix}_invalid_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.InvalidCount);
(metrics[$"{prefix}_rejected_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.RejectedCount);
(metrics[$"{prefix}_sharerate"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Sharerate);
(metrics[$"{prefix}_sharerate_average"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.SharerateAverage);
(metrics[$"{prefix}_solved_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.SolvedCount);
(metrics[$"{prefix}_uptime"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.Uptime);
}


}}
