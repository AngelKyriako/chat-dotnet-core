
namespace ChatApp.WS {

    public class WSMessage<T> {

        public string Action { get; set; }

        public string AccessToken { get; set; }

        public T Body { get; set; }
    }
}
