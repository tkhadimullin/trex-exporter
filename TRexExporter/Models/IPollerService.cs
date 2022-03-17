using Microsoft.Extensions.Hosting;

namespace TrexExporter.Models
{
    public interface IPollerService<TResponse> : IHostedService
    {
        public void InitialiseMetrics(MetricCollection metrics, string prefix);
        public void UpdateMetrics(MetricCollection metrics, TResponse data, string prefix, string host);
    }
}
