using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace TrexExporter
{
    internal class PrometheusExporter : IHostedService
    {
        private readonly KestrelMetricServer _server;

        public PrometheusExporter(IConfiguration configuration)
        {
            _server = new KestrelMetricServer(hostname: configuration.GetValue<string>("exporterHost", "localhost"), 
                                              port: configuration.GetValue<int>("exporterPort", 8088)
            );
        }

        public async Task StartAsync(CancellationToken cancellationToken) => _server.Start();

        public async Task StopAsync(CancellationToken cancellationToken) => await _server.StopAsync();
    }
}