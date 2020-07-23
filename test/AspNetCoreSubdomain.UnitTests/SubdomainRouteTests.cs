using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using Microsoft.Extensions.Options;

namespace AspNetCoreSubdomain.Tests
{
    public class SubdomainRouteTests
    {
        [Fact]
        public void CanParseDefaultsInSubdomain()
        {
            // Arrange & Act
            var subdomainRoute = new SubDomainRoute(
                new[] { "localhost" },
                "{controller=Home}",
                Mock.Of<IRouter>(),
                "default",
                "{action=Index}",
                null,
                null,
                null,
                Mock.Of<IInlineConstraintResolver>(), Mock.Of<IOptions<RouteOptions>>());

            //Assert
            Assert.Equal("Home", subdomainRoute.Defaults["controller"]);
            Assert.Equal("Index", subdomainRoute.Defaults["action"]);
        }
    }
}