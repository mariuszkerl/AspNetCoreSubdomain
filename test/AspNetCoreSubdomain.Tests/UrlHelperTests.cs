
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
using Microsoft.AspNetCore.Routing.Constraints;

namespace AspNetCoreSubdomain.Tests
{
    public class UrlHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void UrlHelperActionReturnsCorrectInlineConstraintInSubdomainUrl(
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
                "{area:bool}",
                "{controller=Home}/{action=Index}");
            ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, subdomain);

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void UrlHelperActionReturnsCorrectParameterConstraintInSubdomainUrl(
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
                "{controller=Home}/{action=Index}",
                null,
                new { area = new BoolRouteConstraint() });
            ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, subdomain);

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstraintInSubdomainTestData))]
        public void UrlHelperW3ActionReturnsCorrectInlineConstraintInSubdomainUrl(
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
                "{area:bool}",
                "{controller=Home}/{action=Index}");
            ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, subdomain);

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstraintInSubdomainTestData))]
        public void UrlHelperW3ActionReturnsCorrectParameterConstraintInSubdomainUrl(
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
                "{controller=Home}/{action=Index}",
                null,
                new { area = new BoolRouteConstraint() });
            ActionContextVisitor.Visit(actionContext, routeBuilder, action, controller, subdomain);

            //Act
            var url = urlHelper.Action(action, controller, new { area = subdomain });

            // Assert
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }

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
        [Theory]
        [MemberData(nameof(MemberDataFactories.W3AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3AreaInSubdomainTestData))]
        public void UrlHelperW3ActionReturnsCorrectAreaInSubdomainUrl(
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
        [MemberData(nameof(MemberDataFactories.W3ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ControllerInSubdomainTestData))]
        public void UrlHelperW3ActionReturnsCorrectControllerInSubdomainUrl(
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
        [MemberData(nameof(MemberDataFactories.W3ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstantSubdomainTestData))]
        public void UrlHelperW3ActionReturnsCorrectConstantInSubdomainUrl(
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
