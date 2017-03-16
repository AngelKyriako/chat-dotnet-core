using System;

using Newtonsoft.Json;

namespace ChatApp.Model {

    public class BaseModel {
        [JsonIgnore]
        public long Key { get; set; }

        public string Id {
            get { return Key.ToString(); }
        }

        public bool Enabled { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
