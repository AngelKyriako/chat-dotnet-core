using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public class WSControllerBase : IWSController {

        private WSConnectionHandler _parentHandler;

        public void SetConnectionHandler(WSConnectionHandler handler) {
            _parentHandler = handler;
        }

        // listening
        public virtual async Task OnConnected(WSConnection connection) {

        }

        public virtual async Task OnDisconnected(WSConnection connection) {

        }

        public virtual async Task OnMessage(WSConnection connection, string message) {

        }

        // emitting
        public async Task SendMessage(string message, Func<WSConnection, bool> filter = null) {
            await _parentHandler.SendMessage(message, filter);
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

    }
}
