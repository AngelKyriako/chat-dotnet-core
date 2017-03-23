using System;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace ChatApp.WS {

    public static class WSMiddlewareExtensions {

        private static bool _initialized = false;
        private static WSConnectionHandlerBuilder _wsBuilder = new WSConnectionHandlerBuilder();

        public static IApplicationBuilder UseWS(this IApplicationBuilder app, Action<WSConnectionHandlerBuilder> configure) {
            app.UseWebSockets();

            if (!_initialized) {
                // The consumer explicitly declares available websocket handlers & controllers.
                configure(_wsBuilder);

                // TODO: Automatically declare available websocket routes, based on IWSControllers' WSRoute attribute.
                //
                // Read All IWSControllers with [WSRoute("/whatever")] attributes
                // Create a seperate WSConnectionHandler for each different route and add the corresponding WSControllers on them.

                _initialized = true;
            }

            IApplicationBuilder lastBuilder = app;
            foreach (PathString route in _wsBuilder.Routes) {
                lastBuilder = app.Map(route, (builder) => builder.UseMiddleware<WSMiddleware>(_wsBuilder.GetConnectionHandler(route)));
                Console.WriteLine("WS route: " + route + " setup.");
            }

            return lastBuilder;
        }

        public static IServiceCollection AddWS(this IServiceCollection services) {
            services.AddSingleton<WSConnectionHolder>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes) {
                if (type.GetTypeInfo().BaseType == typeof(WSConnectionHandler) ||
                    type.GetTypeInfo().GetInterface("IWSConnectionHandler") == typeof(IWSConnectionHandler)) {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

    }
}
