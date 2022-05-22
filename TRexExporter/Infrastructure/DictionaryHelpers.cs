using System.Collections.Generic;

namespace TrexExporter.Infrastructure
{
    public static class DictionaryHelpers
    {
        public static T GetValue<T>(this Dictionary<string, object> dict, string key, T fallback)
        {
            if(dict == null || !dict.ContainsKey(key)) return fallback;
            return (T)dict[key];
        }
    }
}
