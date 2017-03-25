using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Auth {
    using Model;

    public interface IAuthService {

        ICryptoMan Crypto { get; }

        TokenValidationParameters ValidationParams { get; }

        Task<UserAndToken> IssueToken(string username, string password);
        Task<UserAndToken> IssueToken(UserModel user, string password);

        UserModel DecodeTokenToUser(string accessToken);
    }
}
