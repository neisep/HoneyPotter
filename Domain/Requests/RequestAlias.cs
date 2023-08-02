using Newtonsoft.Json;

namespace Domain.Requests
{
    public class RequestAlias
    {
        [JsonProperty("alias")]
        public Alias Alias { get; set; }
        [JsonProperty("network_content")]
        public string Network_content { get; set; }
        [JsonProperty("authgroup_content")]
        public string Authgroup_content { get; set; }
    }
}
