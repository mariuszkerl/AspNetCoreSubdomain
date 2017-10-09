using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Moq;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.WebEncoders.Testing;

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

        public static class OptionsFactory
        {
            public static IOptions<MvcViewOptions> GetMvcViewOptions()
            {
                var mockOptions = new Mock<IOptions<MvcViewOptions>>();
                mockOptions
                    .SetupGet(options => options.Value)
                    .Returns(new MvcViewOptions());

                return mockOptions.Object;
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

        public static class ViewContextFactory
        {
            public static ViewContext Get(
            ActionContext actionContext,
            object model,
            IHtmlGenerator htmlGenerator,
            IModelMetadataProvider metadataProvider,
            ModelStateDictionary modelState)
            {
                var viewData = new ViewDataDictionary(metadataProvider, modelState)
                {
                    Model = model,
                };
                var viewContext = new ViewContext(
                    actionContext,
                    Mock.Of<IView>(),
                    viewData,
                    Mock.Of<ITempDataDictionary>(),
                    TextWriter.Null,
                    new HtmlHelperOptions());

                return viewContext;
            }
        }

        public static class HtmlHelperFactory
        {
            public static HtmlHelper Get(
            Action<IRouteBuilder> mapRoute,
            string host,
            string appRoot,
            string controller,
            string action,
            string area,
            string expectedUrl)
            {
                var services = ConfigurationFactories.ServiceProviderFacotry.Get();
                var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
                var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
                var mvcViewOptions = ConfigurationFactories.OptionsFactory.GetMvcViewOptions();

                mapRoute(routeBuilder);

                var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext, new ActionDescriptor());

                actionContext.RouteData = new RouteData();
                actionContext.RouteData.Values.Add(nameof(action), action);
                actionContext.RouteData.Values.Add(nameof(controller), controller);
                if (area != null)
                {
                    actionContext.RouteData.Values.Add(nameof(area), area);
                }
                actionContext.RouteData.Routers.Add(routeBuilder.Build());

                var metadataProvider = new EmptyModelMetadataProvider();
                var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
                var htmlHelper = new HtmlHelper(htmlGenerator, Mock.Of<ICompositeViewEngine>(), metadataProvider, Mock.Of<IViewBufferScope>(), new HtmlTestEncoder(), UrlTestEncoder.Default);

                //must call Contextualize before using htmlHelper instance
                htmlHelper.Contextualize(ConfigurationFactories.ViewContextFactory.Get(actionContext, null, htmlGenerator, metadataProvider, new ModelStateDictionary()));
                return htmlHelper;
            }
        }
    }
}
