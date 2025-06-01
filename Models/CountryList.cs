using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TrabalhoFinal3.Data
{
    public static class CountryList
    {
        private static List<Country> _cache;

        public static List<Country> All(string physicalPath)
        {
            if (_cache != null) return _cache;

            var json = File.ReadAllText(physicalPath);
            _cache = JsonConvert.DeserializeObject<List<Country>>(json)
                       ?? new List<Country>();
            return _cache;
        }
    }

    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
