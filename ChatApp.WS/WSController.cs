using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public class WSController<M> : IWSController where M : class {

        private WSConnectionHandler _parentHandler;

        public void SetConnectionHandler(WSConnectionHandler handler) {
            _parentHandler = handler;
        }

        // serialization (defaults to JSON)
        public virtual string SerializeMessage(M message) {
            return JsonSerializer.Serialize(message);
        }

        public virtual M DeserializeMessage(string message) {
            return JsonSerializer.Deserialize<M>(message);
        }

        // listening
        public virtual async Task OnConnected(WSConnection connection) {

        }

        public virtual async Task OnDisconnected(WSConnection connection) {

        }

        public virtual async Task OnMessageDeserialized(WSConnection connection, M message) {

        }

        public virtual async Task OnMessageSerialized(WSConnection connection, string message) {

        }

        public async Task OnMessage(WSConnection connection, string message) {
            if (typeof(M) == typeof(string)) return;

            M messageDeserialized = DeserializeMessage(message);

            await OnMessageSerialized(connection, message);

            if (messageDeserialized != null) {
                await OnMessageDeserialized(connection, messageDeserialized);
            }
        }

        // emitting
        public async Task SendMessage(M message, Func<WSConnection, bool> filter = null) {
            await SendMessage(SerializeMessage(message), filter);
        }
        
        public async Task SendMessage(string message, Func<WSConnection, bool> filter = null) {
            await _parentHandler.SendMessage(message, filter);
        }

        public async Task SendMessage(WSConnection connection, M message) {
            await SendMessage(connection, SerializeMessage(message));
        }

        public async Task SendMessage(WSConnection connection, string message) {
            await _parentHandler.SendMessage(connection, message);
        }

        // drop connections
        public async Task DropConnection(WSConnection connection,
            WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure,
            string reason = "connection terminated normally") {
            await _parentHandler.DropConnection(connection, closeStatus, reason);
        }

        public async Task DropConnections(WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure,
            string reason = "connection terminated normally",
            Func<WSConnection, bool> filter = null) {
            await _parentHandler.DropConnections(closeStatus, reason, filter);
        }

        //IFDO: support something like the MVC controller
        //      that would be awesome
        //
        // https://msdn.microsoft.com/en-us/library/mt653985.aspx
        //WSRoute("/ws")
        //public class WSController : IWSController {
        //[WSEvent("MessageSend")]
        //public void SendMessage(string message) {
        //
        //}
        //
        //[WSEvent("MessageSend")]
        //public void SendMessage(MessageModel message) {
        //
        //}
        //
        //[WSEvent("MessageHistoryRequest")]
        //public void GetHistory() {
        //
        //}
    }
}
