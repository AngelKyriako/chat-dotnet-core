﻿using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Swashbuckle.Swagger.Model;

namespace ChatApp.Web {

    using Repository;
    using Repository.Configuration;
    using Auth;
    using Auth.Configuration;
    using Service;
    using Microsoft.AspNetCore.Mvc.Authorization;

    public class Startup {

        public IConfigurationRoot Configuration { get; }
        public string Version { get { return "v" + MajorVersion + "." + MinorVersion + "." + PatchVersion; } }
        public int MajorVersion { get { return Configuration.GetSection("Version").GetValue<int>("Major"); } }
        public int MinorVersion { get { return Configuration.GetSection("Version").GetValue<int>("Minor"); } }
        public int PatchVersion { get { return Configuration.GetSection("Version").GetValue<int>("Patch"); } }

        /// <summary>
        /// Configuration Initialization
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment()) {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
            Console.WriteLine("Starting server " + Version);
        }
        
        /// <summary>
        /// Configures the interfaces that will be availabled via dependency injections thoughout the app.
        /// 
        /// More info: https://www.exceptionnotfound.net/getting-started-with-dependency-injection-in-asp-net-core/
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="services"></param>
        protected void ConfigureInjectedServices<C, K>(IServiceCollection services)
            where C : DbContext
            where K : new() {

            IConfigurationSection database = Configuration.GetSection("Database");
            string databaseType = database.GetValue<string>("Type");
            IConfigurationSection uri = database.GetSection("Uri");
            switch (databaseType) {
                case "MsSql":
                    //TODO: setup mssql
                    services.AddDbContext<C>(opt => opt.UseSqlServer(uri.GetValue<string>(databaseType)));
                    break;
                case "Mongo":
                // TODO: setup mongo & code repository with mongo driver
                case "Memory":
                default:
                    services.AddDbContext<C>(opt => opt.UseInMemoryDatabase());
                    break;
            }
            Console.WriteLine("Startup with " + databaseType + " database.");

            services.AddScoped(typeof(IMessageRepository<K>), typeof(MessageRepository<K>));
            services.AddScoped(typeof(IUserRepository<K>), typeof(UserRepository<K>));

            services.AddSingleton<IAuthService, JwtAuthService<K>>();
            services.AddTransient<IMessageService<K>, MessageService<K>>();
            services.AddTransient<IUserService<K>, UserService<K>>();
        }

        /// <summary>
        /// Services configuration handler.
        /// 
        /// Use this function to create singleton that
        /// can be injected to the app's classes.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddOptions();

            AuthStartup.ConfigureServices(services, Configuration.GetSection("Jwt"));

            services.AddApiVersioning(o => {
                o.DefaultApiVersion = new ApiVersion(MajorVersion, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddSwaggerGen(c => {
                c.SingleApiVersion(new Info {
                    Title = "ChatApp API - v" + MajorVersion,
                    Version = "v" + MajorVersion,
                    Description = "Onion Architecture based chat API with JWT auth, made with dotnet core.",
                    TermsOfService = "Knock yourself out",
                    Contact = new Contact {
                        Name = "Angel Kyriako",
                        Email = "angelkyriako@gmail.com"
                    },
                    License = new License {
                        Name = "Apache 2.0",
                        Url = "http://www.apache.org/licenses/LICENSE-2.0.html"
                    }
                });
            });

            services.AddMvc(config => {
                config.Filters.Add(new AuthorizeFilter(AuthStartup.DefaultPolicy));
            });
            ConfigureInjectedServices<AppEntityContext<long>, long>(services);
        }

        /// <summary>
        /// Middleware Configuration.
        /// The HTTP request pipeline that handles each request received.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ILogger logger = loggerFactory.CreateLogger<Startup>();

            AuthStartup.Configure(app);

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUi("api/ui", "/swagger/v" + MajorVersion + "/swagger.json");

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
