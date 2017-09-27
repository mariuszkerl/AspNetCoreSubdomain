using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Moq;
using System;

namespace TestHelpers
{
    public static class ConfigurationFactories
    {
        public static class RouteBuilderFactory
        {
            public static IRouteBuilder Get(IServiceProvider services)
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
        }

        public static class ActionContextFactory
        {
            public static ActionContext Get(HttpContext context, ActionDescriptor actionDescriptor)
            {
                var actionContext = Get(context);
                actionContext.ActionDescriptor = actionDescriptor;

                return actionContext;
            }
            public static ActionContext Get(HttpContext context)
            {
                return new ActionContext
                {
                    HttpContext = context
                };
            }
        }

        public static class HttpContextFactory
        {
            public static HttpContext Get(IServiceProvider services,
            string host,
            string appRoot)
            {
                var context = new DefaultHttpContext();
                context.RequestServices = services;

                context.Request.PathBase = new PathString(appRoot);
                context.Request.Host = new HostString(host);

                return context;
            }
        }

        public static class ServiceProviderFacotry
        {
           public static IServiceProvider Get()
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
        }
    }
}
