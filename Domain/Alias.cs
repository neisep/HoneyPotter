using Newtonsoft.Json;

namespace Domain
{
    public class Alias
    {
        [JsonProperty("uuid")]
        public Guid Id { get; set; }
        [JsonProperty("enabled")]
        public string Enabled { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("proto")]
        public string Proto { get; set; }
        [JsonProperty("categories")]
        public string Categories { get; set; }
        [JsonProperty("updatefreq")]
        public string Updatefreq { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("interface")]
        public string NetworkInterface { get; set; }
        [JsonProperty("counters")]
        public string Counters { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
