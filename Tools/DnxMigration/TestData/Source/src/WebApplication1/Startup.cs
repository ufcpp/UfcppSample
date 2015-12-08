using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            Xproj();
            Config46();
            Json46();
            Config35();
            Json35();

            app.UseIISPlatformHandler();

            app.Run(async (context) =>
            {
                var message = $@"Hello World!
{Json46()}
{Config46()}
{Json35()}
{Config35()}
{Xproj()}
";

                await context.Response.WriteAsync(message);
            });
        }

        private static string Json46() => new ClassLibraryNet46X.Class1().Name;
        private static string Config46() => new ClassLibraryNet46.Class1().Name;
        private static string Json35() => new ClassLibraryNet35X.Class1().Name;
        private static string Config35() => new ClassLibraryNet35.Class1().Name;
        private static string Xproj() => new XprojLibrary.Class1().Name;

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
