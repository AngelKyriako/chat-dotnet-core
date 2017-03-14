using System;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Auth.Configuration {

    // https://goblincoding.com/2016/07/03/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-i/#next
    public class JwtConfiguration {

        /// <summary>
        /// The security algorithm to be used to generate the signing credentials.
        /// </summary>
        public string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256;

        /// <summary>
        /// The secret key used to generate the SigningCredentials (REQUIRED)
        /// </summary>
        /// <remarks>Should be loaded from a secure location. 
        /// Is used to create the a symmetric security key which
        /// will again be used to generate the signing credentials
        /// based on the Algorithm specified.</remarks>
        public string Secret { get; set; }

        /// <summary>
        /// The signing key to use when generating tokens.
        /// <remarks>Used to authorize user's when trying to access
        /// any resource from the API</remarks>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// "iss" (Issuer) Claim
        /// </summary>
        /// <remarks>The "iss" (issuer) claim identifies the principal that issued the
        ///   JWT.  The processing of this claim is generally application specific.
        ///   The "iss" value is a case-sensitive string containing a StringOrURI
        ///   value.  Use of this claim is OPTIONAL.</remarks>
        public string Issuer { get; set; }

        /// <summary>
        /// "sub" (Subject) Claim
        /// </summary>
        /// <remarks> The "sub" (subject) claim identifies the principal that is the
        ///   subject of the JWT.  The claims in a JWT are normally statements
        ///   about the subject.  The subject value MUST either be scoped to be
        ///   locally unique in the context of the issuer or be globally unique.
        ///   The processing of this claim is generally application specific.  The
        ///   "sub" value is a case-sensitive string containing a StringOrURI
        ///   value.  Use of this claim is OPTIONAL.</remarks>
        public string Subject { get; set; }

        /// <summary>
        /// "aud" (Audience) Claim
        /// </summary>
        /// <remarks>The "aud" (audience) claim identifies the recipients that the JWT is
        ///   intended for.  Each principal intended to process the JWT MUST
        ///   identify itself with a value in the audience claim.  If the principal
        ///   processing the claim does not identify itself with a value in the
        ///   "aud" claim when this claim is present, then the JWT MUST be
        ///   rejected.  In the general case, the "aud" value is an array of case-
        ///   sensitive strings, each containing a StringOrURI value.  In the
        ///   special case when the JWT has one audience, the "aud" value MAY be a
        ///   single case-sensitive string containing a StringOrURI value.  The
        ///   interpretation of audience values is generally application specific.
        ///   Use of this claim is OPTIONAL.</remarks>
        public string Audience { get; set; }

        /// <summary>
        /// "nbf" (Not Before) Claim (default is UTC NOW)
        /// </summary>
        /// <remarks>The "nbf" (not before) claim identifies the time before which the JWT
        ///   MUST NOT be accepted for processing.  The processing of the "nbf"
        ///   claim requires that the current date/time MUST be after or equal to
        ///   the not-before date/time listed in the "nbf" claim.  Implementers MAY
        ///   provide for some small leeway, usually no more than a few minutes, to
        ///   account for clock skew.  Its value MUST be a number containing a
        ///   NumericDate value.  Use of this claim is OPTIONAL.</remarks>
        public DateTime NotBefore { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// "iat" (Issued At) Claim (default is UTC NOW)
        /// </summary>
        /// <remarks>The "iat" (issued at) claim identifies the time at which the JWT was
        ///   issued.  This claim can be used to determine the age of the JWT.  Its
        ///   value MUST be a number containing a NumericDate value.  Use of this
        ///   claim is OPTIONAL.</remarks>
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The timespan of the jwt token's validity
        /// <remards>Indicates how long the jwt token will be valid for authentication,
        /// from the issued date. Once the time span is outdated, the user will need to
        /// request a new token.</remards>
        /// </summary>
        public TimeSpan ValidFor { get; set; }
        public int ValidForMinutes {
            get { return ValidFor.Minutes; }
            set { ValidFor = TimeSpan.FromMinutes(value); }
        }
        public int ValidForHours {
            get { return ValidFor.Hours; }
            set { ValidFor = new TimeSpan(0, value, ValidForMinutes, 0); }
        }
        public int ValidForDays {
            get { return ValidFor.Days; }
            set { ValidFor = new TimeSpan(value, ValidForHours, ValidForMinutes, 0); }
        }

        /// <summary>
        /// "exp" (Expiration Time) Claim (returns IssuedAt + ValidFor)
        /// </summary>
        /// <remarks>The "exp" (expiration time) claim identifies the expiration time on
        ///   or after which the JWT MUST NOT be accepted for processing.  The
        ///   processing of the "exp" claim requires that the current date/time
        ///   MUST be before the expiration date/time listed in the "exp" claim.
        ///   Implementers MAY provide for some small leeway, usually no more than
        ///   a few minutes, to account for clock skew.  Its value MUST be a number
        ///   containing a NumericDate value.  Use of this claim is OPTIONAL.</remarks>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        /// <remarks>The "jti" (JWT ID) claim provides a unique identifier for the JWT.
        ///   The identifier value MUST be assigned in a manner that ensures that
        ///   there is a negligible probability that the same value will be
        ///   accidentally assigned to a different data object; if the application
        ///   uses multiple issuers, collisions MUST be prevented among values
        ///   produced by different issuers as well.  The "jti" claim can be used
        ///   to prevent the JWT from being replayed.  The "jti" value is a case-
        ///   sensitive string.  Use of this claim is OPTIONAL.</remarks>
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());
    }
}
