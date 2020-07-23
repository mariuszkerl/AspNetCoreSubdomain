using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AspNetCoreSubdomain.WebSites.Core;

namespace AspNetCoreSubdomain.RoutesWithoutSubdomain
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSubdomains();
            services.AddMvc(x => x.EnableEndpointRouting = false);

            services.AddScoped<SubdomainRoutingResponseGenerator>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            var hosts = new[] { "localhost" };
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    hostnames: hosts,
                    name: "SubdomainDefault",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
