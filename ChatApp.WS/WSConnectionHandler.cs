using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.WS
{
    public class WSConnectionHandler : IWSConnectionHandler {

        private WSConnectionHolder _wsConnectionHolder;

        /// <summary>
        /// Delegate handler events to IWSController methods
        /// </summary>
        private List<IWSController> _controllers;

        public WSConnectionHandler(WSConnectionHolder wsConnectionHolder) {
            _wsConnectionHolder = wsConnectionHolder;
            _controllers = new List<IWSController>();
        }

        public void AddController(IWSController controller) {
            controller.SetConnectionHandler(this);
            _controllers.Add(controller);
        }

        // listen
        public virtual async Task OnConnected(WebSocket socket) {
            WSConnection connection = _wsConnectionHolder.AddConnectionBySocket(socket);

            foreach (IWSController controller in _controllers) {
                await controller.OnConnected(connection);
            }
        }

        public virtual async Task OnDisconnected(WebSocket socket) {
            WSConnection connection = await _wsConnectionHolder.RemoveConnectionBySocket(socket,
                WebSocketCloseStatus.NormalClosure,
                "disconnected");

            foreach (IWSController controller in _controllers) {
                await controller.OnDisconnected(connection);
            }
        }

        public virtual async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer) {
            WSConnection connection = _wsConnectionHolder.GetConnectionBySocket(socket);

            string payload = Encoding.UTF8.GetString(buffer, 0, result.Count);

            foreach (IWSController controller in _controllers) {
                await controller.OnMessage(connection, payload);
            }
        }

        // emit
        public async Task SendMessage(WSConnection connection, string message) {
            if (connection.Socket.State != WebSocketState.Open)
                return;

            await connection.Socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                      offset: 0,
                                                      count: message.Length),
                       messageType: WebSocketMessageType.Text,
                       endOfMessage: true,
                       cancellationToken: CancellationToken.None);
        }

        public async Task SendMessage(string message, Func<WSConnection, bool> filter) {
            foreach (WSConnection connection in _wsConnectionHolder.Connections) {
                if (filter == null || filter(connection)) {
                    await SendMessage(connection, message);
                }
            }
        }

        // drop connections
        public async Task DropConnection(WSConnection connection, WebSocketCloseStatus closeStatus, string reason) {
            await _wsConnectionHolder.RemoveConnection(connection, closeStatus, reason);
        }

        public async Task DropConnections(WebSocketCloseStatus closeStatus, string reason, Func<WSConnection, bool> filter = null) {
            foreach (WSConnection connection in _wsConnectionHolder.Connections) {
                if (filter == null || filter(connection)) {
                    await DropConnection(connection, closeStatus, reason);
                }
            }
        }
    }
}
