using Newtonsoft.Json;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models { public partial class Shares {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
{$"{prefix}_shares_accepted_count", Metrics.CreateCounter($"{prefix}_shares_accepted_count", "accepted_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_invalid_count", Metrics.CreateCounter($"{prefix}_shares_invalid_count", "invalid_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_last_share_diff", Metrics.CreateGauge($"{prefix}_shares_last_share_diff", "last_share_diff", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_last_share_submit_ts", Metrics.CreateCounter($"{prefix}_shares_last_share_submit_ts", "last_share_submit_ts", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_max_share_diff", Metrics.CreateGauge($"{prefix}_shares_max_share_diff", "max_share_diff", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_max_share_submit_ts", Metrics.CreateCounter($"{prefix}_shares_max_share_submit_ts", "max_share_submit_ts", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_rejected_count", Metrics.CreateCounter($"{prefix}_shares_rejected_count", "rejected_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
{$"{prefix}_shares_solved_count", Metrics.CreateCounter($"{prefix}_shares_solved_count", "solved_count", "host", "slot", "algo", "gpu_id", "vendor", "name") },
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, Dictionary<string, Collector> metrics, Shares data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo);
                                    extraLabels.Insert(0, slot);
                                    extraLabels.Insert(0, host);
                                }
(metrics[$"{prefix}_shares_accepted_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.AcceptedCount);
(metrics[$"{prefix}_shares_invalid_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.InvalidCount);
(metrics[$"{prefix}_shares_last_share_diff"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.LastShareDiff);
(metrics[$"{prefix}_shares_last_share_submit_ts"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.LastShareSubmitTs);
(metrics[$"{prefix}_shares_max_share_diff"] as Gauge).WithLabels(extraLabels.ToArray()).Set(data.MaxShareDiff);
(metrics[$"{prefix}_shares_max_share_submit_ts"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.MaxShareSubmitTs);
(metrics[$"{prefix}_shares_rejected_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.RejectedCount);
(metrics[$"{prefix}_shares_solved_count"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.SolvedCount);
}


}}
