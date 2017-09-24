using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreSubdomain.Tests
{
    public class AnchorTagHelperTests
    {
        [Theory]
        [InlineData("localhost", "/", "area1", "Home", "Index", "http://area1.localhost/")]
        [InlineData("localhost", "/", "area1", "Home", "About", "http://area1.localhost/Home/About")]
        [InlineData("localhost", "/", "area1", "Test", "Index", "http://area1.localhost/Test")]
        [InlineData("localhost", "/", "area1", "Test", "About", "http://area1.localhost/Test/About")]
        [InlineData("area1.localhost", "/", "area1", "Home", "Index", "/")]
        [InlineData("area1.localhost", "/", "area1", "Home", "About", "/Home/About")]
        [InlineData("area1.localhost", "/", "area1", "Test", "Index", "/Test")]
        [InlineData("area1.localhost", "/", "area1", "Test", "About", "/Test/About")]
        async public void CanCreateSubdomainAreaTagHelper(
            string host,
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
                ActionDescriptor = new ActionDescriptor()
            };

            actionContext.HttpContext = CreateHttpContext(services, host, appRoot);

            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("action", action);
            actionContext.RouteData.Values.Add("controller", controller);
            actionContext.RouteData.Values.Add("area", subdomain);
            actionContext.RouteData.Routers.Add(routeBuilder.Build());
            
            var metadataProvider = new EmptyModelMetadataProvider();
            var htmlGenerator = new DefaultHtmlGenerator(Mock.Of<IAntiforgery>(), GetOptions(), metadataProvider, new SubdomainUrlHelperFactory(), new HtmlTestEncoder(), new ClientValidatorCache(), new DefaultValidationHtmlAttributeProvider(GetOptions(), metadataProvider, new ClientValidatorCache()));
            var helper = new Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper(htmlGenerator)
            {
                ViewContext = GetViewContext(actionContext, null, htmlGenerator, metadataProvider, new ModelStateDictionary()),
                Action = action,
                Controller = controller,
                Host = host,
                Area = subdomain
            };
            var context = new TagHelperContext(
               allAttributes: new TagHelperAttributeList(
                   new[] { new TagHelperAttribute("a") }),
               items: new Dictionary<object, object>(),
               uniqueId: "test-id");
            var output = new TagHelperOutput(
                "a",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            //Act
            helper.Process(context, output);

            //Assert
            Assert.Empty(output.Content.GetContent());
            Assert.Equal(1, output.Attributes.Count);
            Assert.Equal("href", output.Attributes.First().Name);
            Assert.Equal(expectedUrl, output.Attributes.First().Value);
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
            string host,
            string appRoot)
        {
            var context = new DefaultHttpContext();
            context.RequestServices = services;

            context.Request.PathBase = new PathString(appRoot);
            context.Request.Host = new HostString(host);

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

        private static IOptions<MvcViewOptions> GetOptions()
        {
            var mockOptions = new Mock<IOptions<MvcViewOptions>>();
            mockOptions
                .SetupGet(options => options.Value)
                .Returns(new MvcViewOptions());

            return mockOptions.Object;
        }

        ViewContext GetViewContext(
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
}
