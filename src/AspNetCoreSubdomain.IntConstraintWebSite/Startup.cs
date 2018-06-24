using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSubdomain.WebSites.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSubdomain.IntConstraintWebSite
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSubdomains();
            services.AddMvc();

            services.AddScoped<SubdomainRoutingResponseGenerator>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            var hosts = new[] { "localhost" };
            app.UseMvc(routes =>
            {
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter:int}",
                    name: "IntSubdomain",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter:bool}",
                    name: "BoolSubdomain",
                    template: "{controller=Home}/{action=Boolean}/{id?}");
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter:decimal}",
                    name: "test",
                    template: "{controller=Home}/{action=Boolean}/{id?}");
            });
        }
    }
}
