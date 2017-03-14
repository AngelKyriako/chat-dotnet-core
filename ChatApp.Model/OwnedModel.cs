namespace ChatApp.Model {
    
    public class OwnedModel<K> : BaseModel<K> {
        public K CreatorId { get; set; }
        public UserModel<K> Creator { get; set; }
    }
}
