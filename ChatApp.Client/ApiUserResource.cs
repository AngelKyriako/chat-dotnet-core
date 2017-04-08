using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Client {
    using Model;

    public class ApiUserResource : ApiResource<UserModel> {

        private const string RESOURCE_NAME = "user";

        public ApiUserResource(IApiClient parent, string version)
            : base(parent, version, RESOURCE_NAME) {
        }

        public ApiUserResource(IApiClient parent)
            : base(parent, RESOURCE_NAME) {
        }

        public override async Task<UserModel> Post(UserModel user) {
            throw new NotSupportedException("Normal Post is not supported for user, use Register instead.");
        }

        public async Task<UserAndToken> Register(UserModel user) {
            if (user == null) {
                throw new ArgumentException("a valid UserModel is needed to register.");
            }
            Parent.Session = await _request.Post<UserModel, UserAndToken>(_route, user);
            return Parent.Session;
        }

        public async Task<UserAndToken> Login(LocalCredentials credentials) {
            if (credentials == null) {
                throw new ArgumentException("valid LocalCredentials are needed to login.");
            }
            Parent.Session = await _request.Post<LocalCredentials, UserAndToken>(Path.Combine(_route, "login"), credentials);
            return Parent.Session;
        }

        public void Logout() {
            Parent.Session = null;
        }
    }
}
