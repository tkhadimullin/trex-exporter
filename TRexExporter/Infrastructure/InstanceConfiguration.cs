using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace TrexExporter.Infrastructure
{
    public class InstanceConfiguration : IConfiguration
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();
        public string this[string key] { get => _values[key]; set => _values[key] = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            //var v = new ConfigurationSection(this, "")
            return new List<ConfigurationSection>();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
