using AspNetCoreSubdomain.WebSites.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreSubdomain.ParameterConstraintsWebSite
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
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter}",
                    name: "IntSubdomain",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { parameter = new IntRouteConstraint() });
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter}",
                    name: "LongSubdomain",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Long" },
                    constraints: new { parameter = new LongRouteConstraint() });
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter}",
                    name: "BoolSubdomain",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Boolean" },
                    constraints: new { parameter = new BoolRouteConstraint() });
                routes.MapSubdomainRoute(
                    hostnames: hosts,
                    subdomain: "{parameter}",
                    name: "GuidSubdomain",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Guid" },
                    constraints: new { parameter = new GuidRouteConstraint() });
            });
        }
    }
}
