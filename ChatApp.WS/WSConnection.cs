using System.Net.WebSockets;

namespace ChatApp.WS {

    public class WSConnection {

        public string Id { get; set; }

        public WebSocket Socket { get; set; }

        public override string ToString() {
            return "WSConnection: " + Id;
        }
    }
}
