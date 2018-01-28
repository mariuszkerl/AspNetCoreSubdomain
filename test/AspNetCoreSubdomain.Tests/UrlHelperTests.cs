
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
using TestHelpers;

namespace AspNetCoreSubdomain.Tests
{
    public class UrlHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaInSubdomainTestData))]
        public void UrlHelperActionReturnsCorrectAreaInSubdomainUrl(
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
            var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext);
            var urlHelper = new SubdomainUrlHelper(actionContext);

            routeBuilder.MapSubdomainRoute(
                new[] { "example.com" },
                "default",
                "{area}",
                "{controller=Home}/{action=Index}");
            ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, subdomain);

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ControllerInSubdomainTestData))]
        public void UrlHelperActionReturnsCorrectControllerInSubdomainUrl(
            string host,
            string appRoot,
            string subdomain,
            string action,
            string expectedUrl)
        {
            // Arrange
            var services = ConfigurationFactories.ServiceProviderFacotry.Get();
            var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
            var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
            var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext);
            var urlHelper = new SubdomainUrlHelper(actionContext);

            routeBuilder.MapSubdomainRoute(
                new[] { "example.com" },
                "default",
                "{controller}",
                "{action=Index}");

            ActionContextVisitor.Visit(actionContext, routeBuilder, action, subdomain, null);

            //Act
            var url = urlHelper.Action(action, subdomain);

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstantSubdomainTestData))]
        public void UrlHelperActionReturnsCorrectConstantInSubdomainUrl(
            string host,
            string appRoot,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            var services = ConfigurationFactories.ServiceProviderFacotry.Get();
            var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
            var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
            var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext);
            var urlHelper = new SubdomainUrlHelper(actionContext);

            routeBuilder.MapSubdomainRoute(
                new[] { "example.com" },
                "default",
                "constantsubdomain",
                "{controller=Home}/{action=Index}");
            ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, null);

            //Act
            var url = urlHelper.Action(action, controller);

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
    }
}
