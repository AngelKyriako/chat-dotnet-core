using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
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
                controller.OnConnected(connection);
            }
        }

        public virtual async Task OnDisconnected(WebSocket socket) {
            WSConnection connection = await _wsConnectionHolder.RemoveConnectionBySocket(socket);

            foreach (IWSController controller in _controllers) {
                controller.OnDisconnected(connection);
            }
        }

        public virtual async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer) {
            WSConnection connection = _wsConnectionHolder.GetConnectionBySocket(socket);

            string payload = Encoding.UTF8.GetString(buffer, 0, result.Count);
            foreach (IWSController controller in _controllers) {
                controller.OnMessage(connection, payload);
            }
        }

        // emit
        public async Task SendMessageAsync(string message, Func<WSConnection, bool> filter) {
            foreach (WSConnection connection in _wsConnectionHolder.Connections) {
                if (connection.Socket.State == WebSocketState.Open && filter(connection)) {
                    await connection.SendMessage(message);
                }
            }
        }
    }
}
