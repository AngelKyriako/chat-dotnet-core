using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Auth {
    using Model;

    public interface IAuthService {

        string Issuer { get; }

        string Audience { get; }

        SymmetricSecurityKey SigningKey { get; }

        Task<JwtTokenModel> IssueToken(string username, string password);
    }
}
