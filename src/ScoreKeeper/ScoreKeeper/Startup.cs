using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace ScoreKeeper
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure self-hosted Web API 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            appBuilder.UseWebApi(config);

            // Enable self-hosted SignalR
            appBuilder.Map("/signalr", map =>
            {
                var hubConfiguration = new HubConfiguration
                {
                };

                map.RunSignalR(hubConfiguration);
            });

            // Enable file server for static files
            appBuilder.Use(typeof(MiddleWare));
            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                EnableDefaultFiles = true,
                DefaultFilesOptions = { DefaultFileNames = { "index.html" } },
                FileSystem = new PhysicalFileSystem("Web")
            };
            appBuilder.UseFileServer(options);
        }
    }
    class MiddleWare : OwinMiddleware
    {
        public MiddleWare(OwinMiddleware next)
        : base(next)
        {
        }
        public override async Task Invoke(IOwinContext context)
        {
            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
            await Next.Invoke(context);
        }
    }
}
