
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using System;
using System.Text.Encodings.Web;
using Xunit;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace AspNetCoreSubdomain.Tests
{
    public class UrlHelperTests
    {
        [Theory]
        [InlineData("/", "area1", "Home", "Index", "http://area1.localhost/")]
        [InlineData("/", "area1", "Home", "About", "http://area1.localhost/Home/About")]
        [InlineData("/", "area1", "Test", "Index", "http://area1.localhost/Test")]
        [InlineData("/", "area1", "Test", "About", "http://area1.localhost/Test/About")]
        public void UrlHelperActionReturnsCorrectSubdomainUrl(
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            var services = CreateServices();
            var routeBuilder = CreateRouteBuilder(services);

            routeBuilder.MapSubdomainRoute(
                new[] { "localhost" },
                "default",
                "{area}",
                "{controller=Home}/{action=Index}");

            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    RequestServices = services,
                },
            };

            actionContext.HttpContext = CreateHttpContext(services, appRoot);

            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("action", action);
            actionContext.RouteData.Values.Add("controller", controller);
            actionContext.RouteData.Values.Add("area", subdomain);
            actionContext.RouteData.Routers.Add(routeBuilder.Build());

            var urlHelper = new SubdomainUrlHelper(actionContext);

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddLogging();
            services.AddRouting();
            services
                //exceptitons are thrown without ObjectPoolProvider
                .AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            return services.BuildServiceProvider();
        }

        private static HttpContext CreateHttpContext(
            IServiceProvider services,
            string appRoot)
        {
            var context = new DefaultHttpContext();
            context.RequestServices = services;

            context.Request.PathBase = new PathString(appRoot);
            context.Request.Host = new HostString("localhost");

            return context;
        }

        private static ActionContext CreateActionContext(HttpContext context, IRouter router)
        {
            var routeData = new RouteData();
            routeData.Routers.Add(router);

            return new ActionContext(context, routeData, new ActionDescriptor());
        }

        private static IRouteBuilder CreateRouteBuilder(IServiceProvider services)
        {
            var app = new Mock<IApplicationBuilder>();
            app
                .SetupGet(a => a.ApplicationServices)
                .Returns(services);

            return new RouteBuilder(app.Object)
            {
                //That's needed for route mappings. It cannot be empty for constructor,
                //will just pass requests further
                DefaultHandler = new DefaultRouter(),
            };
        }

        private class DefaultRouter : IRouter
        {
            public VirtualPathData GetVirtualPath(VirtualPathContext context)
            {
                return null;
            }

            public Task RouteAsync(RouteContext context)
            {
                context.Handler = (c) => Task.FromResult(0);
                return Task.FromResult(false);
            }
        }
    }
}
