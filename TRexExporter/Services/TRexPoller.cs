using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TrexExporter.Models.TRex;

namespace TrexExporter.Services
{
    public class TRexPoller : BasePollerService<TRexResponse>
    {
        public TRexPoller(Dictionary<string, object> minerConfig, IConfiguration config, MetricCollection metrics) : base(minerConfig, config, metrics) { }

        protected override string PollUrl => "summary";

        public override void InitialiseMetrics(MetricCollection metrics, string prefix)
        {
            foreach (var (key, value) in TRexResponse.GetMetrics(prefix)) metrics.TryAdd(key, value);
            foreach (var (key, value) in DualStat.GetMetrics(prefix)) metrics.TryAdd(key, value);
            foreach (var (key, value) in Gpu.GetMetrics(prefix)) metrics.TryAdd(key, value);
            foreach (var (key, value) in Shares.GetMetrics(prefix)) metrics.TryAdd(key, value);
        }
        public override void UpdateMetrics(MetricCollection metrics, TRexResponse data, string prefix, string host)
        {
            TRexResponse.UpdateMetrics(prefix, metrics, data, host, "main", data.Algorithm);

            foreach (var dataGpu in data.Gpus)
            {
                Gpu.UpdateMetrics(prefix, metrics, dataGpu, host, "main", data.Algorithm, new List<string>
                {
                    dataGpu.DeviceId.ToString(),
                    dataGpu.Vendor,
                    dataGpu.Name
                });
                Shares.UpdateMetrics(prefix, metrics, dataGpu.Shares, host, "main", data.Algorithm, new List<string>
                {
                    dataGpu.GpuId.ToString(),
                    dataGpu.Vendor,
                    dataGpu.Name
                });

                if (data.DualStat != null)
                {
                    DualStat.UpdateMetrics(prefix, metrics, data.DualStat, host, "dual", data.DualStat.Algorithm);
                    var dualStatGpu = data.DualStat.Gpus.Find(c => c.DeviceId == dataGpu.DeviceId);
                    Gpu.UpdateMetrics(prefix, metrics, dualStatGpu, host, "dual", data.DualStat.Algorithm, new List<string>
                    {
                        dataGpu.DeviceId.ToString(),
                        dataGpu.Vendor,
                        dataGpu.Name
                    });
                    Shares.UpdateMetrics(prefix, metrics, dualStatGpu.Shares, host, "dual", data.DualStat.Algorithm, new List<string>
                    {
                        dataGpu.DeviceId.ToString(),
                        dataGpu.Vendor,
                        dataGpu.Name
                    });
                }
            }
        }
    }
}