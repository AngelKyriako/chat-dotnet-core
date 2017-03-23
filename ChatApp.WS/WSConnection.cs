using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public class WSConnection {

        public string Id { get; set; }

        public WebSocket Socket { get; set; }

        public async Task SendMessage(string message) {
            if (Socket.State != WebSocketState.Open)
                return;

            await Socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                              offset: 0,
                                                                              count: message.Length),
                                               messageType: WebSocketMessageType.Text,
                                               endOfMessage: true,
                                               cancellationToken: CancellationToken.None);
        }

        public override string ToString() {
            return "WSConnection: " + Id;
        }
    }
}
