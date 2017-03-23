using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ChatApp.WS {

    public class WSConnectionHandlerBuilder {

        private Dictionary<PathString, IWSConnectionHandler> _mappings;

        public IEnumerable<PathString> Routes { get { return _mappings.Keys; } }

        public WSConnectionHandlerBuilder() {
            _mappings = new Dictionary<PathString, IWSConnectionHandler>();
        }

        public void AddConnectionHandler(PathString route, IWSConnectionHandler handler) {
            if (route == null) {
                throw new ArgumentNullException("route should not be null");
            }
            if (handler == null) {
                throw new ArgumentNullException("handler should not be null");
            }
            if (_mappings.ContainsKey(route)) {
                throw new ArgumentException("route: \"" + route + "\" already added in builder");
            }
            _mappings.Add(route, handler);
        }

        public IWSConnectionHandler GetConnectionHandler(PathString route) {
            if (route == null) {
                throw new ArgumentNullException("route should not be null");
            }
            if (!_mappings.ContainsKey(route)) {
                throw new ArgumentException("route: \"" + route + "\" does not exist in builder");
            }
            return _mappings[route];
        }
    }
}
