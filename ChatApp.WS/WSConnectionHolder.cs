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

        public async Task<WSConnection> RemoveConnectionBySocket(WebSocket socket) {
            if (socket == null) {
                throw new ArgumentNullException("socket should not be null");
            }

            string connectionId = _connections.FirstOrDefault(p => p.Value.Socket == socket).Key;

            return await RemoveConnectionById(connectionId);
        }

        public async Task<WSConnection> RemoveConnectionById(string connectionId) {
            if (connectionId == null) {
                throw new ArgumentNullException("connectionId should not be null");
            }

            WSConnection connection;
            _connections.TryRemove(connectionId, out connection);

            await connection.Socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by " + typeof(WSConnectionHolder).Name,
                                    cancellationToken: CancellationToken.None);

            return connection;
        }

        public async Task<WSConnection> RemoveConnection(WSConnection connection) {
            if (connection == null) {
                throw new ArgumentNullException("connection should not be null");
            }

            return await RemoveConnectionById(connection.Id);
        }
    }

}
