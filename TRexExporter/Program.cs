using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TrexExporter
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args)
                .Build();
            
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((configuration) =>
                {
                    configuration.Sources.Clear();
                    
                    configuration
                        .AddEnvironmentVariables("TREX")
                        .AddCommandLine(args);
                    
                })
                .ConfigureServices(c =>
                {
                    c.AddSingleton<MetricCollection>();
                    c.AddHostedService<PrometheusExporter>();
                    c.AddHostedService<TRexPoller>();
                })
                .ConfigureLogging(c => c.AddConsole())
            ;
    }
}
