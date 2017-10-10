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
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                    new StringWriter(),
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
                ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, area);

                var metadataProvider = new EmptyModelMetadataProvider();
                var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
                var htmlHelper = new HtmlHelper(htmlGenerator, Mock.Of<ICompositeViewEngine>(), metadataProvider, Mock.Of<IViewBufferScope>(), new HtmlTestEncoder(), UrlTestEncoder.Default);

                //must call Contextualize before using htmlHelper instance
                htmlHelper.Contextualize(ConfigurationFactories.ViewContextFactory.Get(actionContext, null, metadataProvider, new ModelStateDictionary()));
                return htmlHelper;
            }
        }

        public static class TagHelperFactory
        {
            public static TagHelper GetAnchor(
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
                ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, area);

                var metadataProvider = new EmptyModelMetadataProvider();
                var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
                var tagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper(htmlGenerator)
                {
                    ViewContext = ConfigurationFactories.ViewContextFactory.Get(actionContext, null, metadataProvider, new ModelStateDictionary()),
                    Action = action,
                    Controller = controller,
                    Host = host
                };

                if (area != null)
                {
                    tagHelper.Area = area;
                }

                return tagHelper;
            }
            public static TagHelper GetForm(
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
                ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, area);

                var metadataProvider = new EmptyModelMetadataProvider();
                var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
                var tagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper(htmlGenerator)
                {
                    ViewContext = ConfigurationFactories.ViewContextFactory.Get(actionContext, null, metadataProvider, new ModelStateDictionary()),
                    Action = action,
                    Controller = controller,
                    Antiforgery = false
                };

                if (area != null)
                {
                    tagHelper.Area = area;
                }

                return tagHelper;
            }
        }

        public static class TagHelperContextFactory
        {
            public static TagHelperContext Get()
            {
                return new TagHelperContext(
                     allAttributes: new TagHelperAttributeList(),
                     items: new Dictionary<object, object>(),
                     uniqueId: "test-id");
            }
        }
        public static class TagHelperOutputFactory
        {
            public static TagHelperOutput GetAnchor()
            {
                return GetOutput("a");
            }
            public static TagHelperOutput GetForm()
            {
                return GetOutput("form");
            }
            private static TagHelperOutput GetOutput(string tagName)
            {
                return new TagHelperOutput(
                    tagName,
                    attributes: new TagHelperAttributeList(),
                    getChildContentAsync: (useCachedResult, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    });
            }
        }
    }
}
