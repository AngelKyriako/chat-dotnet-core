using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public class WSController : IWSController {

        private WSConnectionHandler _parentHandler;

        public void SetConnectionHandler(WSConnectionHandler handler) {
            _parentHandler = handler;
        }

        // listen
        public virtual async Task OnConnected(WSConnection connection) {
        }

        public virtual async Task OnDisconnected(WSConnection connection) {
        }

        public virtual async Task OnMessage(WSConnection connection, string message) {
        }

        // emit
        public virtual async Task SendMessageToAll(string message) {
            await _parentHandler.SendMessageToAllAsync(message);
        }

        //TODO: support something like this
        //
        // https://msdn.microsoft.com/en-us/library/mt653985.aspx
        //WSRoute("/ws")
        //public class WSController : IWSController {
        //[WSEvent("MessageSend")]
        //public void SendMessage(string message) {

        //}

        //[WSEvent("MessageSend")]
        //public void SendMessage(MessageModel message) {

        //}

        //[WSEvent("MessageHistoryRequest")]
        //public void GetHistory() {

        //}
    }
}
