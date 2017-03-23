using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatApp.Model {

    /// <summary>
    /// Handles JSON serialization & deserialization based on Newtonsoft.Json.
    /// 
    /// The goal is that this class handles deserialization exactly like dotnet core MVC
    /// So that it can be used for websocket calls.
    /// 
    /// More info: https://medium.com/@agriciuc/working-with-json-net-in-net-core-rc2-part-1-5f3a65f4e11#.cv3wxfkp9
    /// </summary>
    public static class JsonSerializer {

        private static readonly JsonSerializerSettings DEFAULT_SETTINGS = new JsonSerializerSettings() {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static M Deserialize<M>(string json) {
            return JsonConvert.DeserializeObject<M>(json);
        }

        public static string Serialize(object model) {
            return JsonConvert.SerializeObject(model, DEFAULT_SETTINGS);
        }
    }
}
