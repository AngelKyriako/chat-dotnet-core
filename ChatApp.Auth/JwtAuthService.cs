using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Auth {
    using Configuration;
    using Model;
    using Repository;

    /// <summary>
    /// Handles Authentication & Authorization of the application via Json Web Tokens.
    /// 
    /// References:
    /// goblin coding part 1: https://goblincoding.com/2016/07/03/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-i/
    /// goblin coding part 2: https://goblincoding.com/2016/07/07/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-ii/
    ///
    /// decrypt: https://andrewlock.net/a-look-behind-the-jwt-bearer-authentication-middleware-in-asp-net-core/
    /// </summary>
    public class JwtAuthService : IAuthService {

        private ILogger _logger;
        private IUserRepository _users;
        private JwtConfiguration _config;
        private SymmetricSecurityKey _signingKey;

        public ICryptoMan Crypto { get; private set; }

        public JwtAuthService(ILoggerFactory loggerFactory, IOptions<JwtConfiguration> config,
                              IUserRepository users, ICryptoMan crypto) {

            _logger = loggerFactory.CreateLogger<JwtAuthService>();
            _config = config.Value;
            _users = users;

            Crypto = crypto;

            _logger.LogInformation("Generating signing credentials with " + _config.Algorithm);
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.Secret));
            _config.SigningCredentials = new SigningCredentials(_signingKey, _config.Algorithm);

            ThrowIfInvalidOptions(_config);

            _logger.LogInformation("Issuer: " + _config.Issuer 
                                 + ", Audience: " + _config.Audience 
                                 + ", token validFor timeframe: " + _config.ValidFor);

            ValidationParams = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = _config.Issuer,

                ValidateAudience = true,
                ValidAudience = _config.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    }

        private static void ThrowIfInvalidOptions(JwtConfiguration options) {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero) {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtConfiguration.ValidFor));
            }

            if (options.SigningCredentials == null) {
                throw new ArgumentNullException(nameof(JwtConfiguration.SigningCredentials));
            }

            if (options.JtiGenerator == null) {
                throw new ArgumentNullException(nameof(JwtConfiguration.JtiGenerator));
            }
        }

        /// <summary>
        /// Converts date to seconds, since Unix epoch (Jan 1, 1970, midnight UTC).
        /// </summary>
        /// <param name="date"></param>
        /// <returns>epoch date to seconds</returns>
        private static long ToUnixEpochDate(DateTime date) {
            return (long)Math.Round(
                (date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }

        public TokenValidationParameters ValidationParams { get; private set; }

        public async Task<UserAndToken> IssueToken(UserModel user, string password) {
            if (user == null || password == null ||
                user.PasswordHash != Crypto.HashWithSalt(password, user.PasswordSalt)) {
                _logger.LogDebug("password hashed to -> " + Crypto.HashWithSalt(password, user.PasswordSalt));
                _logger.LogDebug("DB password hash -> " + user.PasswordHash);
                _logger.LogDebug("DB password salt -> " + user.PasswordSalt);
                throw new ArgumentException($"Invalid username or password");
            }
            string jti = await _config.JtiGenerator();

            List<Claim> claims = new List<Claim> {
                new Claim(AuthConst.CLAIM_ID, user.Id),
                new Claim(AuthConst.CLAIM_JTI, jti),
                new Claim(AuthConst.CLAIM_IAT, ToUnixEpochDate(_config.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(AuthConst.CLAIM_USERNAME, user.Username),
                new Claim(AuthConst.CLAIM_FIRSTNAME, user.Firstname),
                new Claim(AuthConst.CLAIM_LASTNAME, user.Lastname)
            };
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.Username, "JwtAccessToken"), claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            // Create the JWT security token and encode it.
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: principal.Claims,
                notBefore: _config.NotBefore,
                expires: _config.Expiration,
                signingCredentials: _config.SigningCredentials
            );

            // Export to a model ready for the consumer
            JwtToken token = new JwtToken() {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresInSeconds = (int)_config.ValidFor.TotalSeconds
            };

            return new UserAndToken() {
                User = user,
                Token = token
            };
        }

        public UserModel DecodeTokenToUser(string accessToken) {
            JwtSecurityTokenHandler validator = new JwtSecurityTokenHandler();

            if (validator.CanReadToken(accessToken)) {
                SecurityToken accessTokenDecoded;
                ClaimsPrincipal principal = validator.ValidateToken(accessToken, ValidationParams, out accessTokenDecoded);
                _logger.LogInformation("principal.Identity.Name: " + principal.Identity.Name);
                if (principal.Identity.IsAuthenticated) {
                    return _users.GetOneEnabledByUsername(principal.Claims.SingleOrDefault((claim) => {
                        return claim.Type == AuthConst.CLAIM_USERNAME;
                    }).Value);
                }
            }

            return null;
        }

        public async Task<UserAndToken> IssueToken(string username, string password) {
            return await IssueToken(_users.GetOneByUsername(username), password);
        }
    }
}
