using System;

using Newtonsoft.Json;

namespace ChatApp.Model {

    public class BaseModel {

        [JsonIgnore]
        public long Key { get; set; }

        public string Id {
            get { return Key.ToString(); }
        }

        [JsonIgnore]
        public bool Enabled { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public bool IsValid { get { return Id != null && Id.Length != 0; } }
        
        public string ToJson() {
            return JsonSerializer.Serialize(this);
        }
    }
}
