using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestHelpers;
using Xunit;
using System.IO;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace AspNetCoreSubdomain.Tests
{
    public class ActionLinkHtmlHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void CanCreateInlineConstraintSubdomainActionLinkHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{parameter:int}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { parameter = subdomain });
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void CanCreateParameterConstraintSubdomainActionLinkHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{parameter}",
                    "{controller=Home}/{action=Index}",
                    null,
                    new { parameter = new BoolRouteConstraint() });
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { parameter = subdomain });
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaInSubdomainTestData))]
        public void CanCreateAreaSubdomainActionLinkHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain });
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ControllerInSubdomainTestData))]
        public void CanCreateControllerSubdomainActionLinkHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{controller}",
                    "{action=Index}");
            }, host, appRoot, subdomain, action, null, expectedUrl);
            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: subdomain);
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstantSubdomainTestData))]
        public void CanCreateConstantActionLinkHtmlHelper(
            string host,
            string appRoot,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "constantsubdomain",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, null, expectedUrl);
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


        [Theory]
        [MemberData(nameof(MemberDataFactories.W3AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3AreaInSubdomainTestData))]
        public void CanCreateW3AreaSubdomainActionLinkHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain });
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ControllerInSubdomainTestData))]
        public void CanCreateW3ControllerSubdomainActionLinkHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{controller}",
                    "{action=Index}");
            }, host, appRoot, subdomain, action, null, expectedUrl);
            // Act
            var actualLink = htmlHelper.ActionLink(
                                            linkText: "Test",
                                            actionName: action,
                                            controllerName: subdomain);
            string resultHtml;
            using (var writer = new StringWriter())
            {
                actualLink.WriteTo(writer, new HtmlTestEncoder());
                resultHtml = writer.ToString();
            }

            // Assert
            Assert.Equal(expectedLink, resultHtml);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstantSubdomainTestData))]
        public void CanCreateW3ConstantActionLinkHtmlHelper(
            string host,
            string appRoot,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedLink = $"<a href=\"HtmlEncode[[{expectedUrl}]]\">HtmlEncode[[Test]]</a>";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "constantsubdomain",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, null, expectedUrl);
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
