using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrexExporter.Models;

namespace TrexExporter.Services
{

    public abstract class BasePollerService<TResponse> : IPollerService<TResponse>
    {
        protected readonly IConfiguration _configuration;
        private readonly MetricCollection _metrics;
        private readonly HttpClient _client;

        protected abstract string Prefix { get; }
        protected abstract string Host { get; }
        private bool _stopping;
        protected abstract string PollUrl { get; }

        protected BasePollerService(IConfiguration configuration, MetricCollection metrics)
        {
            _configuration = configuration;
            _metrics = metrics;
            _client = new HttpClient { BaseAddress = new Uri(Host) };
            InitialiseMetrics(_metrics, Prefix);
        }

        public abstract void InitialiseMetrics(MetricCollection metrics, string prefix);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => {
                while (!cancellationToken.IsCancellationRequested && !_stopping)
                {
                    try
                    {
                        var response = await _client.GetStringAsync(PollUrl);
                        var data = JsonConvert.DeserializeObject<TResponse>(response);
                        UpdateMetrics(_metrics, data, Prefix, Host);
                        Thread.Sleep(_configuration.GetValue<int>("pollInterval", 5000));
                    }
                    catch (Exception e)
                    {
                        // TODO: error handling
                    }
                }
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _stopping = true;
        }

        public abstract void UpdateMetrics(MetricCollection metrics, TResponse data, string prefix, string host);
    }
}
