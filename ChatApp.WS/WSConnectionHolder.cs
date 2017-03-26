using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public class WSConnectionHolder {

        private ConcurrentDictionary<string, WSConnection> _connections;

        public IEnumerable<WSConnection> Connections {
            get { return _connections.Values; }
        }

        public WSConnectionHolder() {
            _connections = new ConcurrentDictionary<string, WSConnection>();
        }

        private string CreateConnectionId() {
            return Guid.NewGuid().ToString();
        }

        // Accessors
        public WebSocket GetSocketByConnectionId(string connectionId) {
            if (connectionId == null) {
                throw new ArgumentNullException("connectionId should not be null");
            }

            return _connections.FirstOrDefault(p => p.Key == connectionId).Value.Socket;
        }

        public WebSocket GetSocketByConnection(WSConnection connection) {
            if (connection == null) {
                throw new ArgumentNullException("connection should not be null");
            }

            return GetSocketByConnectionId(connection.Id);
        }

        public WSConnection GetConnectionBySocket(WebSocket socket) {
            return _connections.FirstOrDefault(p => p.Value.Socket == socket).Value;
        }

        public WSConnection GetConnectionById(string connectionId) {
            return _connections.FirstOrDefault(p => p.Key == connectionId).Value;
        }

        // Mutators
        public WSConnection AddConnectionBySocket(WebSocket socket) {
            if (socket == null) {
                throw new ArgumentNullException("socket should not be null");
            }
            WSConnection connection = new WSConnection() {
                Id = CreateConnectionId(),
                Socket = socket
            };
            _connections.TryAdd(connection.Id, connection);

            return connection;
        }

        public async Task<WSConnection> RemoveConnectionById(string connectionId, WebSocketCloseStatus closeStatus, string reason) {
            if (connectionId == null) {
                throw new ArgumentNullException("connectionId should not be null");
            }

            WSConnection connection;
            _connections.TryRemove(connectionId, out connection);

            if (connection.Socket.State != WebSocketState.Closed &&
                connection.Socket.State != WebSocketState.CloseSent &&
                connection.Socket.State != WebSocketState.CloseReceived) {
                await connection.Socket.CloseAsync(closeStatus, reason, CancellationToken.None);
            }

            return connection;
        }

        public async Task<WSConnection> RemoveConnectionBySocket(WebSocket socket, WebSocketCloseStatus closeStatus, string reason) {

            if (socket == null) {
                throw new ArgumentNullException("socket should not be null");
            }

            string connectionId = _connections.FirstOrDefault(p => p.Value.Socket == socket).Key;

            return await RemoveConnectionById(connectionId, closeStatus, reason);
        }

        public async Task<WSConnection> RemoveConnection(WSConnection connection, WebSocketCloseStatus closeStatus, string reason) {

            if (connection == null) {
                throw new ArgumentNullException("connection should not be null");
            }

            return await RemoveConnectionById(connection.Id, closeStatus, reason);
        }
    }

}
