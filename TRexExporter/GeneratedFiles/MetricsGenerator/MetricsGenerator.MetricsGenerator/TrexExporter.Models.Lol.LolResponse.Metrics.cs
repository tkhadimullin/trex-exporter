using Newtonsoft.Json;
using System.Collections.Generic;
using TrexExporter.Metadata;

using System; 
using System.Collections.Generic;
using Prometheus; 


namespace TrexExporter.Models.Lol { public partial class LolResponse {

public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {
};
                            return result;
                        }

public static void UpdateMetrics(string prefix, MetricCollection metrics, LolResponse data, string host, string slot, string algo, List<string> extraLabels = null) {
if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }
}


}}
