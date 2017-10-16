using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var x = new Lib.Class1();
                x.PropertyChanged += (sender, args) =>
                {
                    ((Lib.Class1)sender).Id = 123;
                };

                x.Secret = "";
                await context.Response.WriteAsync("Hello World! " + x.Id);
                await context.Response.WriteAsync("\n" + x.GetType().Assembly.FullName);
                foreach (var i in x.GetType().GetInterfaces())
                {
                    await context.Response.WriteAsync("\n    : " + i.FullName);
                }
            });
        }
    }
}
