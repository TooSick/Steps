using System.Collections.Generic;
using System.Text.Json;
using Steps.Model;
using System.IO;

namespace Steps
{
    public static class Deserializer
    {
        public static List<Person> DeserializingData(string file)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
            };
            string data = File.ReadAllText(file);

            return new List<Person>(JsonSerializer.Deserialize<List<Person>>(data));
        }
    }
}
