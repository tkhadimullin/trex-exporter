using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrexExporter.Infrastructure;

namespace TrexExporter
{
    class Program
    {
        public static async Task Main(string[] args)
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
                        .AddEnvironmentVariables("MININGSTATS_")
                        .AddCommandLine(args);
                })
                .ConfigureServices(c =>
                {
                    c.AddSingleton<MetricCollection>();
                    c.AddHostedService<PrometheusExporter>();
                    c.InstantiateMiningPollerServices("MINER_");
                })
                .ConfigureLogging(c => c.AddConsole())
            ;
    }
}
