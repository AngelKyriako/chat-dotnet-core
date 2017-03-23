using System;

using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.WS {
    /// <summary>
    /// Example usage of the middleware in a Startup.cs 
    /// part of an dotnet core MVC app.
    /// </summary>
    public class WSStartup {

        public static void ConfigureServices(IServiceCollection services) {
            //services.AddWS();
        }

        public static void Configure(IApplicationBuilder app, IServiceProvider serviceProvider) {
            //app.UseWS((builder) => {
            //    IWSConnectionHandler mainHandler = serviceProvider.GetService<WSConnectionHandler>();
            //    mainHandler.AddController(serviceProvider.GetService<SocketController>());

            //    builder.AddConnectionHandler("/ws", mainHandler);
            //    builder.AddConnectionHandler("/ws/other", mainHandler);
            //});
        }
    }
}
