using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public string Issuer { get { return _config.Issuer; } }

        public string Audience { get { return _config.Audience; } }

        public SymmetricSecurityKey SigningKey { get { return _signingKey; } }

        //TODO: Fix JWT authorization. :/
        public async Task<UserAndToken> IssueToken(UserModel user, string password) {
            if (user == null || password == null || !Crypto.HashEqualsString(user.PasswordHash, password)) {
                throw new ArgumentException($"Invalid username or password");
            }
            string jti = await _config.JtiGenerator();

            List<Claim> claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_config.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Firstname),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Lastname)
            };

            //ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.Username, "JwtAccessToken"), claims);

            // Create the JWT security token and encode it.
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                notBefore: _config.NotBefore,
                expires: _config.Expiration,
                signingCredentials: _config.SigningCredentials
            );

            #region DELETE ME 
            _logger.LogInformation("JWT: ");
            foreach (var c in jwt.Claims) {
                _logger.LogInformation("Claim: " + c.Type + ": " + c.Value + " of type " + c.ValueType);
            }

            var test = new JwtSecurityTokenHandler();
            string accessToken = test.WriteToken(jwt);
            JwtSecurityToken decoded = test.ReadJwtToken(accessToken);


            _logger.LogInformation("~~~~~~~~~~~~~~~~~~~~ TEST JWT ~~~~~~~~~~~~~~~~~~");
            _logger.LogInformation("test.CanValidateToken" + test.CanValidateToken);
            _logger.LogInformation("test.CanWriteToken" + test.CanWriteToken);
            _logger.LogInformation("test.CanReadToken(accessToken)" + test.CanReadToken(accessToken));
            _logger.LogInformation("Decoded JWT: ");
            foreach (var c in decoded.Claims) {
                _logger.LogInformation("Claim " + c.Type + ": " + c.Value + " of type " + c.ValueType);
            }
            _logger.LogInformation("decoded.Actor: " + decoded.Actor);
            _logger.LogInformation("~~~~~~~~~~~~~~~~~~~~ END ~~~~~~~~~~~~~~~~~~");
            #endregion

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

        public async Task<UserAndToken> IssueToken(string username, string password) {
            return await IssueToken(_users.GetOneByUsername(username), password);
        }
    }
}
