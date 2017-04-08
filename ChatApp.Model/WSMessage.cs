namespace ChatApp.Model {

    public class WSMessage<M> {

        public string Action { get; set; }

        public string AccessToken { get; set; }

        public M Payload { get; set; }
    }
}