using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Prometheus;
using TrexExporter.Models;

namespace TrexExporter
{
    internal class MetricCollection
    {
        private readonly Dictionary<string, Collector> _metrics;
        private readonly string _prefix;
        private readonly string _host;

        public MetricCollection(IConfiguration configuration)
        {
            _prefix = configuration.GetValue<string>("exporterPrefix", "trex");
            _metrics = new Dictionary<string, Collector>();
            _host = configuration.GetValue<string>("baseUrl", "http://192.168.1.253:4067");
            foreach (var (key, value) in TRexResponse.GetMetrics(_prefix))
            {
                _metrics.Add(key, value);
            }
            foreach (var (key, value) in DualStat.GetMetrics(_prefix))
            {
                _metrics.Add(key, value);
            }
            foreach (var (key, value) in Gpu.GetMetrics(_prefix))
            {
                _metrics.Add(key, value);
            }
            foreach (var (key, value) in Shares.GetMetrics(_prefix))
            {
                _metrics.Add(key, value);
            }
        }

        public void Update(TRexResponse data)
        {
            TRexResponse.UpdateMetrics(_prefix, _metrics, data, _host, "main", data.Algorithm);
            DualStat.UpdateMetrics(_prefix, _metrics, data.DualStat, _host, "dual", data.DualStat.Algorithm);
            
            foreach (var dataGpu in data.Gpus)
            {
                Gpu.UpdateMetrics(_prefix, _metrics, dataGpu, _host, "main", data.Algorithm, new List<string>
                {
                    dataGpu.DeviceId.ToString(),
                    dataGpu.Vendor,
                    dataGpu.Name
                });
                Shares.UpdateMetrics(_prefix, _metrics, dataGpu.Shares, _host, "main", data.Algorithm, new List<string>
                {
                    dataGpu.GpuId.ToString(),
                    dataGpu.Vendor,
                    dataGpu.Name
                });

                var dualStatGpu = data.DualStat.Gpus.Find(c => c.DeviceId == dataGpu.DeviceId);
                Gpu.UpdateMetrics(_prefix, _metrics, dualStatGpu, _host, "dual", data.DualStat.Algorithm, new List<string>
                {
                    dataGpu.DeviceId.ToString(),
                    dataGpu.Vendor,
                    dataGpu.Name
                });
                Shares.UpdateMetrics(_prefix, _metrics, dualStatGpu.Shares, _host, "dual", data.DualStat.Algorithm, new List<string>
                {
                    dataGpu.DeviceId.ToString(),
                    dataGpu.Vendor,
                    dataGpu.Name
                });

            }
        }
    }
}