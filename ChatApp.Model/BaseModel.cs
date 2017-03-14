using System;

namespace ChatApp.Model {

    public class BaseModel<K> {
        public K Id { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
