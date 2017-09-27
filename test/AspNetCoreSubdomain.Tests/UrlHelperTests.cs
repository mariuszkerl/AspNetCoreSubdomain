
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
        [MemberData(nameof(MemberDataFactories.AreaSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaSubdomainTestData))]
        public void UrlHelperActionReturnsCorrectAreaSubdomainUrl(
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
                new[] { "localhost" },
                "default",
                "{area}",
                "{controller=Home}/{action=Index}");

            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("action", action);
            actionContext.RouteData.Values.Add("controller", controller);
            actionContext.RouteData.Values.Add("area", subdomain);
            actionContext.RouteData.Routers.Add(routeBuilder.Build());

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
    }
}
