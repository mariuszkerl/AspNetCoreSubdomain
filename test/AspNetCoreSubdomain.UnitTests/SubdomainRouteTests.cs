
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
                Mock.Of<IInlineConstraintResolver>());

            //Assert
            Assert.Equal("Home", subdomainRoute.Defaults["controller"]);
            Assert.Equal("Index", subdomainRoute.Defaults["action"]);
        }
    }
}