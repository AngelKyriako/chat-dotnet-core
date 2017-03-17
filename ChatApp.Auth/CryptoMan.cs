using System;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatApp.Auth {
    using Configuration;

    public class CryptoMan : ICryptoMan {

        private ILogger _logger;
        private CryptoConfiguration _config;

        public CryptoMan(ILoggerFactory loggerFactory, IOptions<CryptoConfiguration> config) {
            _logger = loggerFactory.CreateLogger<CryptoMan>();
            _config = config.Value;
        }

        public byte[] GenerateSalt() {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(salt);
            }
            _logger.LogDebug($"Salt: {Convert.ToBase64String(salt)}");
            return salt;
        }

        public string HashWithSalt(string str, byte[] salt) {
            if (str == null) {
                throw new ArgumentNullException("undefined string value");
            }
            if (salt == null) {
                throw new ArgumentNullException("undefined salt value");
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
            );

            return hashed;
        }
    }
}
