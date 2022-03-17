using System.Collections.Generic;
using Newtonsoft.Json;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models.TRex { public partial class DualStat {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
{$"{prefix}_dual_stat_accepted_count", Metrics.CreateCounter($"{prefix}_dual_stat_accepted_count", "accepted_count", "host", "slot", "algo") },
{$"{prefix}_dual_stat_hashrate", Metrics.CreateGauge($"{prefix}_dual_stat_hashrate", "hashrate", "host", "slot", "algo") },
{$"{prefix}_dual_stat_hashrate_day", Metrics.CreateGauge($"{prefix}_dual_stat_hashrate_day", "hashrate_day", "host", "slot", "algo") },
{$"{prefix}_dual_stat_hashrate_hour", Metrics.CreateGauge($"{prefix}_dual_stat_hashrate_hour", "hashrate_hour", "host", "slot", "algo") },
{$"{prefix}_dual_stat_hashrate_minute", Metrics.CreateGauge($"{prefix}_dual_stat_hashrate_minute", "hashrate_minute", "host", "slot", "algo") },
{$"{prefix}_dual_stat_invalid_count", Metrics.CreateCounter($"{prefix}_dual_stat_invalid_count", "invalid_count", "host", "slot", "algo") },
{$"{prefix}_dual_stat_rejected_count", Metrics.CreateCounter($"{prefix}_dual_stat_rejected_count", "rejected_count", "host", "slot", "algo") },
{$"{prefix}_dual_stat_sharerate", Metrics.CreateGauge($"{prefix}_dual_stat_sharerate", "sharerate", "host", "slot", "algo") },
{$"{prefix}_dual_stat_sharerate_average", Metrics.CreateGauge($"{prefix}_dual_stat_sharerate_average", "sharerate_average", "host", "slot", "algo") },
{$"{prefix}_dual_stat_solved_count", Metrics.CreateCounter($"{prefix}_dual_stat_solved_count", "solved_count", "host", "slot", "algo") },
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, MetricCollection metrics, DualStat data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }
(metrics[$"{prefix}_dual_stat_accepted_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.AcceptedCount);
(metrics[$"{prefix}_dual_stat_hashrate"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Hashrate);
(metrics[$"{prefix}_dual_stat_hashrate_day"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateDay);
(metrics[$"{prefix}_dual_stat_hashrate_hour"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateHour);
(metrics[$"{prefix}_dual_stat_hashrate_minute"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.HashrateMinute);
(metrics[$"{prefix}_dual_stat_invalid_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.InvalidCount);
(metrics[$"{prefix}_dual_stat_rejected_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.RejectedCount);
(metrics[$"{prefix}_dual_stat_sharerate"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.Sharerate);
(metrics[$"{prefix}_dual_stat_sharerate_average"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.SharerateAverage);
(metrics[$"{prefix}_dual_stat_solved_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.SolvedCount);
}


}}
