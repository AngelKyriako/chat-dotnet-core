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
    /// 
    /// More info on policies and claims:
    /// Barry-Dorrans repo: https://github.com/blowdart/AspNetAuthorizationWorkshop
    /// Barry-Dorrans part 1: https://channel9.msdn.com/Blogs/Seth-Juarez/ASPNET-Core-Authorization-with-Barry-Dorrans
    /// Barry-Dorrans part 2: https://channel9.msdn.com/Blogs/Seth-Juarez/Advanced-aspNET-Core-Authorization-with-Barry-Dorrans
    /// </summary>
    public static class AuthStartup {

        private static void ConfigureRegularUserPolicy(AuthorizationPolicyBuilder policyBuilder) {
            policyBuilder
                .RequireClaim(AuthConst.CLAIM_ID)
                .RequireClaim(AuthConst.CLAIM_JTI)
                .RequireClaim(AuthConst.CLAIM_IAT)
                .RequireClaim(AuthConst.CLAIM_USERNAME)
                .RequireClaim(AuthConst.CLAIM_FIRSTNAME)
                .RequireClaim(AuthConst.CLAIM_LASTNAME);
        }

        public static AuthorizationPolicy DefaultPolicy {
            get {
                AuthorizationPolicyBuilder builder = new AuthorizationPolicyBuilder();

                builder.RequireAuthenticatedUser();
                ConfigureRegularUserPolicy(builder);

                return builder.Build();
            }
        }

        public static void ConfigureServices(IServiceCollection services, IConfigurationSection config) {
            services.Configure<JwtConfiguration>(config);

            services.AddAuthorization(options => {
                
                // Available policies to use for authorization
                options.AddPolicy(AuthConst.POLICY_REGULAR_USER, policy => {
                    policy.RequireAuthenticatedUser();

                    ConfigureRegularUserPolicy(policy);
                });

                options.AddPolicy(AuthConst.POLICY_ADMIN, policy => {
                    policy.RequireAuthenticatedUser();

                    ConfigureRegularUserPolicy(policy);

                    policy.RequireClaim(AuthConst.CLAIM_IS_ADMIN);
                 });

                options.AddPolicy(AuthConst.POLICY_PRO_USER, policy => {
                    policy.RequireAuthenticatedUser();

                    ConfigureRegularUserPolicy(policy);

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
                TokenValidationParameters = authService.ValidationParams
            });
        }

    }
}
