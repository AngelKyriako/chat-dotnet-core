namespace ChatApp.Model {
    
    public class MessageModel<K> : OwnedModel<K> {
        public string Body { get; set; }

        public void Copy(MessageModel<K> other) {
            Body = other.Body;
        }
    }
}
