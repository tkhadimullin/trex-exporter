using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrexExporter.Infrastructure;
using TrexExporter.Models;

namespace TrexExporter.Services
{

    public abstract class BasePollerService<TResponse> : IPollerService<TResponse>
    {
        protected readonly Dictionary<string, object> _minerConfig;
        private readonly IConfiguration _generalConfig;
        private readonly MetricCollection _metrics;
        private readonly HttpClient _client;        

        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        protected string Prefix => _minerConfig.GetValue<string>("exporterprefix", "");
        protected string Host => _minerConfig.GetValue<string>("baseurl", "http://127.0.0.1:4000");
        protected abstract string PollUrl { get; }


        protected BasePollerService(Dictionary<string, object> minerConfig, IConfiguration generalConfig, MetricCollection metrics)
        {
            _minerConfig = minerConfig;
            _generalConfig = generalConfig;
            _metrics = metrics;
            _client = new HttpClient { BaseAddress = new Uri(Host) };
            InitialiseMetrics(_metrics, Prefix);
        }

        public abstract void InitialiseMetrics(MetricCollection metrics, string prefix);

        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _client.GetStringAsync(PollUrl);
                    var data = JsonConvert.DeserializeObject<TResponse>(response);
                    UpdateMetrics(_metrics, data, Prefix, Host);                    
                }
                catch (Exception)
                {
                    // TODO: error handling
                }
                await Task.Delay(_generalConfig.GetValue<int>("pollInterval", 5000), cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public abstract void UpdateMetrics(MetricCollection metrics, TResponse data, string prefix, string host);
    }
}
