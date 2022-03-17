using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TrexExporter.Models.Lol;
using TrexExporter.Services;

namespace TrexExporter
{
    public class LolMinerPoller : BasePollerService<LolResponse>
    {
        private string _vendorOverride;
        private string _nameOverride;

        public LolMinerPoller(IConfiguration configuration, MetricCollection metrics): base(configuration, metrics)
        {
        }

        protected override string PollUrl => "summary";
        protected override string Prefix => _configuration.GetValue<string>("lolExporterPrefix", "");
        protected override string Host => _configuration.GetValue<string>("lolBaseUrl", "http://127.0.0.1:4000");

        public override void InitialiseMetrics(MetricCollection metrics, string prefix)
        {
            _vendorOverride = _configuration.GetValue<string>("lolGpuVendorOverride", "");
            _nameOverride = _configuration.GetValue<string>("lolGpuNameOverride", "");

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