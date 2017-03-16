using System;

using Newtonsoft.Json;

namespace ChatApp.Model {
    
    public class OwnedModel : BaseModel {
        [JsonIgnore]
        public long CreatorKey { get; set; }

        public string CreatorId {
            get { return CreatorKey.ToString(); }
            set {
                try {
                    CreatorKey = long.Parse(value);
                } catch(Exception) {
                    CreatorKey = 0;
                }
            }
        }
        public UserModel Creator { get; set; }
    }
}
