using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreSubdomain.WebSites.Core;

namespace AspNetCoreSubdomain.SubdomainsAreaWebSite
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                    subdomain:"{area}",
                    name: "SubdomainDefault",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
