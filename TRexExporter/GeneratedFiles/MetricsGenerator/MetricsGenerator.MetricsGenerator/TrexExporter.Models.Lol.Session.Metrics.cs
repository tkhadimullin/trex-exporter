using Newtonsoft.Json;
using System.Collections.Generic;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models.Lol { public partial class Session {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
{$"{prefix}_shares_uptime", Metrics.CreateCounter($"{prefix}_shares_uptime", "uptime", "host", "slot", "algo") },
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, MetricCollection metrics, Session data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }
(metrics[$"{prefix}_shares_uptime"] as Counter).WithLabels(extraLabels.ToArray()).IncTo(data.Uptime);
}


}}
