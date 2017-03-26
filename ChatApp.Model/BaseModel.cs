using System;

using Newtonsoft.Json;

namespace ChatApp.Model {

    public class BaseModel {

        [JsonIgnore]
        public long Key { get; set; }

        public string Id {
            get { return Key.ToString(); }
            set {
                long keyRef = -1;
                long.TryParse(value, out keyRef);
                if (keyRef != -1) {
                    Key = keyRef;
                }
            }
        }

        [JsonIgnore]
        public bool Enabled { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public bool IsValid { get { return Id != null && Id.Length != 0; } }
    }
}
