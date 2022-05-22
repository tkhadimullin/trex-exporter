using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrexExporter.Services;

namespace TrexExporter.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection InstantiateMiningPollerServices(this IServiceCollection serviceCollection, string prefix) {
            var data = new Dictionary<int, Dictionary<string, object>>();

            foreach (var val in Environment.GetEnvironmentVariables()
                                            .Cast<DictionaryEntry>().Where(s => ((string)s.Key).StartsWith(prefix)))
            {
                var key = ((string)val.Key).Substring(prefix.Length).ToLowerInvariant();
                var parsed = Regex.Matches(key, "([0-9]+)_(.+)").First().Groups;

                var minerNumber = int.Parse(parsed[1].Value);
                var parameterName = parsed[2].Value;
                if (!data.ContainsKey(minerNumber))
                {
                    var firstValue = new Dictionary<string, object> {
                        { parameterName, val.Value }
                    };
                    data.Add(minerNumber, firstValue);
                }
                else {
                    data[minerNumber].Add(parameterName, val.Value);
                }
            }

            foreach (var key in data.Keys) {
                if (!data[key].ContainsKey("type")) throw new Exception($"Сonfiguratrion for {key} requires miner type");
                switch (data[key]["type"].ToString().ToLowerInvariant()) {
                    case "lol":
                    case "lolminer":
                    case "lolminerpoller":
                        serviceCollection.AddSingleton<IHostedService, LolMinerPoller>(sp => new LolMinerPoller(data[key], sp.GetService<IConfiguration>(), sp.GetService<MetricCollection>()));
                        break;
                    case "trex":
                    case "trexpoller":
                        serviceCollection.AddSingleton<IHostedService, TRexPoller>(sp => new TRexPoller(data[key], sp.GetService<IConfiguration>(), sp.GetService<MetricCollection>()));
                        break;
                    default: 
                        throw new Exception($"Unknown miner type {data[key]["type"]}");
            }
            }                        
            return serviceCollection;
        }
    }
}
