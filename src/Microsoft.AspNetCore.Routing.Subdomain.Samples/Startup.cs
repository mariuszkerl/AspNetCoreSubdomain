using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Routing.Subdomain.Samples
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                var hostnames = new[] { "localhost" };
                routes.MapSubdomainRoute(
                    hostnames,
                    "Example3",
                    "{yourParameterName2}",
                    "{id}",
                    new { controller = "Home", action = "Parameters" });
                routes.MapSubdomainRoute(
                    hostnames,
                    "Example1",
                    "{yourParameterName}",
                    "{controller}/{action}",
                    new { controller = "Home", action = "ParameterFromSubdomain" });

                //static subdomains have to be defined as last
                routes.MapSubdomainRoute(
                    hostnames,
                    "Example4",
                    "katowice",
                    "{controller}/{action}/{id}",
                    new { controller = "Home", action = "StaticSubdomain" });
                routes.MapSubdomainRoute(
                    hostnames,
                    "Example2",
                    "katowice",
                    "{controller}/{action}",
                    new { controller = "Home", action = "StaticSubdomain" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id}");
                routes.MapRoute(
                    "route",
                    "{controller}/{action}",
                    new { controller = "Home", action = "StaticSubdomain" }
                    );
            });
        }
    }
}
