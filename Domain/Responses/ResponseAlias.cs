using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class ResponseAlias
    {
        [JsonProperty("rows")]
        public List<Alias> Aliases { get; set; }
    }
}
