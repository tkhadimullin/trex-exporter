using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using TrexExporter.Models;

namespace TrexExporter
{
    internal class TRexPoller : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly MetricCollection _metrics;
        private readonly HttpClient _client;
        private bool _stopping;

        public TRexPoller(IConfiguration configuration, MetricCollection metrics)
        {
            _configuration = configuration;
            _metrics = metrics;
            _client = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("baseUrl", "http://192.168.1.253:4067")),
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!_stopping)
            {
                var response = await _client.GetStringAsync("summary");
                var data = JsonConvert.DeserializeObject<TRexResponse>(response);
                _metrics.Update(data);
                Thread.Sleep(_configuration.GetValue<int>("pollInterval", 5000));
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken) => _stopping = true;
    }
}