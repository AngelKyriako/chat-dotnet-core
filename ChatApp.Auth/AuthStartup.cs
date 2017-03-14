using System;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Auth.Configuration {
    
    /// <summary>
    /// Includes Any Authentication and Authorization constants and
    /// middleware static methods to be called in the Startup script
    /// of an dotnet code MVC app.
    /// </summary>
    public static class AuthStartup {

        private static void ConfigureRegularUserPolicy(AuthorizationPolicyBuilder policyBuilder) {
            policyBuilder.RequireClaim(JwtRegisteredClaimNames.Sub)
                .RequireClaim(JwtRegisteredClaimNames.Jti)
                .RequireClaim(JwtRegisteredClaimNames.Iat)
                .RequireClaim(JwtRegisteredClaimNames.UniqueName)
                .RequireClaim(JwtRegisteredClaimNames.GivenName)
                .RequireClaim(JwtRegisteredClaimNames.FamilyName);
        }

        public static AuthorizationPolicy DefaultPolicy {
            get {
                AuthorizationPolicyBuilder builder = new AuthorizationPolicyBuilder();

                builder.RequireAuthenticatedUser();
                //ConfigureRegularUserPolicy(builder);

                return builder.Build();
            }
        }

        public static void ConfigureServices(IServiceCollection services, IConfigurationSection config) {
            services.Configure<JwtConfiguration>(config);

            services.AddAuthorization(options => {
                
                // Available policies to use for authorization

                options.AddPolicy(AuthConst.POLICY_REGULAR_USER, policy => {
                    //TODO: Fix JWT authorization. :/
                    ConfigureRegularUserPolicy(policy);
                    // default policy for all resources
                    //options.DefaultPolicy = policy.Build();
                });

                options.AddPolicy(AuthConst.POLICY_ADMIN, policy => {
                    policy.RequireClaim(AuthConst.CLAIM_IS_ADMIN);
                 });

                options.AddPolicy(AuthConst.POLICY_PRO_USER, policy => {
                    policy.RequireClaim(AuthConst.CLAIM_MEMBERSHIP, 
                            AuthConst.CLAIM_MEMBERSHIP_VALUE_BASIC,
                            AuthConst.CLAIM_MEMBERSHIP_VALUE_MORE_GUNS);
                });
            });
        }

        public static void Configure(IApplicationBuilder app) {
            IAuthService authService = app.ApplicationServices.GetService<IAuthService>();

            app.UseJwtBearerAuthentication(new JwtBearerOptions {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = authService.Issuer,

                    ValidateAudience = true,
                    ValidAudience = authService.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authService.SigningKey,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                }
            });
        }

    }
}
