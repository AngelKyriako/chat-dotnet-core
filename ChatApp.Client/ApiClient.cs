using System;

namespace ChatApp.Client {
    using Model;

    public class ApiClient : IApiClient {

        private UserAndToken _session;

        public Uri ServerUrl { get; private set; }

        public string Version { get; private set; }

        public UserAndToken Session {
            get { return _session; }
            set {
                _session = value;

                User.Session = _session;
                Message.Session = _session;
            }
        }

        public bool IsAuthenticated { get { return Session != null; } }

        public ApiUserResource User { get; private set; }
        public ApiMessageResource Message { get; private set; }

        public ApiClient(Uri serverUrl, string version) {
            _session = null;

            ServerUrl = serverUrl;
            Version = version;

            User = new ApiUserResource(this);
            Message = new ApiMessageResource(this);
        }

        public ApiClient(string serverUrl, string version) : this(new Uri(serverUrl), version) {
        }
    }
}
