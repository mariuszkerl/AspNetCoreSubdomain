using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelpers;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System.IO;
using System.Buffers;
using System.Globalization;
using Microsoft.Extensions.WebEncoders.Testing;

namespace AspNetCoreSubdomain.Tests
{
    public class ActionLinkHtmlHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstantSubdomainTestData))]
        public void CanCreateConstantSubdomainAnchorTagHelper(
            string host,
            string appRoot,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var services = ConfigurationFactories.ServiceProviderFacotry.Get();
            var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
            var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
            var mvcViewOptions = ConfigurationFactories.OptionsFactory.GetMvcViewOptions();

            routeBuilder.MapSubdomainRoute(
                new[] { "localhost" },
                "default",
                "constantsubdomain",
                "{controller=Home}/{action=Index}");

            var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext, new ActionDescriptor());

            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("action", action);
            actionContext.RouteData.Values.Add("controller", controller);
            actionContext.RouteData.Routers.Add(routeBuilder.Build());

            var metadataProvider = new EmptyModelMetadataProvider();
            var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
            var htmlHelper = new HtmlHelper(htmlGenerator, Mock.Of<ICompositeViewEngine>(), metadataProvider, Mock.Of<IViewBufferScope>(), new HtmlTestEncoder(), UrlTestEncoder.Default);

            //must call Contextualize before using htmlHelper instance
            htmlHelper.Contextualize(ConfigurationFactories.ViewContextFactory.Get(actionContext, null, htmlGenerator, metadataProvider, new ModelStateDictionary()));

            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: controller);
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }
    }
}
