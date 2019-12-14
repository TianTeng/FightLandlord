using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BetGame.DDZ.WasmClient.Model
{
    public class APIReturn
    {
        [JsonPropertyName("code")] public int Code { get;  set; }
        [JsonPropertyName("message")] public string Message { get;  set; }
        [JsonPropertyName("data")] public Dictionary<string,object> Data { get;  set; }
        [JsonPropertyName("success")] public bool Success { get { return this.Code == 0; } }
    }
}
