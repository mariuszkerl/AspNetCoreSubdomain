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
using TestHelpers;
using Xunit;

namespace AspNetCoreSubdomain.Tests
{
    public class AnchorTagHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.AreaSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaSubdomainTestData))]
        async public void CanCreateSubdomainAreaTagHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            var services = ConfigurationFactories.ServiceProviderFacotry.Get();
            var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
            var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
            routeBuilder.MapSubdomainRoute(
                new[] { "localhost" },
                "default",
                "{area}",
                "{controller=Home}/{action=Index}");

            var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext, new ActionDescriptor());

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
