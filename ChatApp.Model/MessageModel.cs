namespace ChatApp.Model {
    
    public class MessageModel : OwnedModel {

        public string Body { get; set; }

        public void Copy(MessageModel other) {
            Body = other.Body;
        }
    }
}
