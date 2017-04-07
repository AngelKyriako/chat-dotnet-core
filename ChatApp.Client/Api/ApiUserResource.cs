using System;
using System.Threading.Tasks;

namespace ChatApp.Client {
    using Model;

    public class ApiUserResource : ApiResource<UserModel> {
        public ApiUserResource(Uri serverUrl) : base(serverUrl, "user") {
        }

        public override async Task<UserModel> Post(UserModel user) {
            throw new NotSupportedException("Normal Post is not supported for user, use Register instead.");
        }

        public async Task<UserAndToken> Register(UserModel user) {
            if (user == null) {
                throw new ArgumentException("a valid UserModel is needed to register.");
            }
            return await _request.Post<UserModel, UserAndToken>(_route, user);
        }

        public async Task<UserAndToken> Login(LocalCredentials credentials) {
            if (credentials == null) {
                throw new ArgumentException("valid LocalCredentials are needed to login.");
            }
            return await _request.Post<LocalCredentials, UserAndToken>(_route, credentials);
        }
    }
}
