using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TrexExporter.Infrastructure;
using TrexExporter.Models.Lol;

namespace TrexExporter.Services
{
    public class LolMinerPoller : BasePollerService<LolResponse>
    {
        private string _vendorOverride;
        private string _nameOverride;

        public LolMinerPoller(Dictionary<string, object> configuration, IConfiguration generalConfig, MetricCollection metrics) : base(configuration, generalConfig, metrics) { }

        protected override string PollUrl => "summary";


        public override void InitialiseMetrics(MetricCollection metrics, string prefix)
        {
            _vendorOverride = _minerConfig.GetValue("gpuvendoroverride", "");
            _nameOverride = _minerConfig.GetValue("gpunameoverride", "");

            foreach (var (key, value) in Session.GetMetrics(prefix)) metrics.TryAdd(key, value);
            foreach (var (key, value) in GPU.GetMetrics(prefix)) metrics.TryAdd(key, value);
        }

        public override void UpdateMetrics(MetricCollection metrics, LolResponse data, string prefix, string host)
        {
            Session.UpdateMetrics(prefix, metrics, data.Session, host, "main", data.Mining.Algorithm);

            foreach (var dataGpu in data.GPUs)
            {
                GPU.UpdateMetrics(prefix, metrics, dataGpu, host, "main", data.Mining.Algorithm, new List<string>
                {
                    dataGpu.Index.ToString(),
                    _vendorOverride,
                    string.IsNullOrEmpty(_nameOverride) ? dataGpu.Name : _nameOverride
                });
            }
        }
    }
}