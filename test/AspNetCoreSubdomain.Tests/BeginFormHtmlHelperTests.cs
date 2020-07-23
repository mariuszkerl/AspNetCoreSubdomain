using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestHelpers;
using Xunit;
using System.IO;
using Microsoft.AspNetCore.Routing.Constraints;

namespace AspNetCoreSubdomain.Tests
{
    public class BeginFormHtmlHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void CanCreateInlineConstraintSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area:bool}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain },
                                            method: FormMethod.Post,
                                            antiforgery: false,
                                            htmlAttributes: null);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void CanCreateParameterConstraintSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}",
                    null,
                    new { area = new BoolRouteConstraint() });
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain },
                                            method: FormMethod.Post,
                                            antiforgery: false,
                                            htmlAttributes: null);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstraintInSubdomainTestData))]
        public void CanCreateW3InlineConstraintSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area:bool}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain },
                                            method: FormMethod.Post,
                                            antiforgery: false,
                                            htmlAttributes: null);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstraintInSubdomainTestData))]
        public void CanCreateW3ParameterConstraintSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}",
                    null,
                    new { area = new BoolRouteConstraint() });
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain },
                                            method: FormMethod.Post,
                                            antiforgery: false,
                                            htmlAttributes: null);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaInSubdomainTestData))]
        public void CanCreateAreaSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain },
                                            method: FormMethod.Post,
                                            antiforgery: false,
                                            htmlAttributes: null);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ControllerInSubdomainTestData))]
        public void CanCreateControllerSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{controller}",
                    "{action=Index}");
            }, host, appRoot, subdomain, action, null, expectedUrl);

            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: subdomain);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
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
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "constantsubdomain",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, null, expectedUrl);

            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }
        [Theory]
        [MemberData(nameof(MemberDataFactories.W3AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3AreaInSubdomainTestData))]
        public void CanCreateW3AreaSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller,
                                            routeValues: new { area = subdomain },
                                            method: FormMethod.Post,
                                            antiforgery: false,
                                            htmlAttributes: null);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.W3ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ControllerInSubdomainTestData))]
        public void CanCreateW3ControllerSubdomainBeginFormHtmlHelper(
            string host,
            string appRoot,
            string subdomain,
            string action,
            string expectedUrl)
        {
            // Arrange
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{controller}",
                    "{action=Index}");
            }, host, appRoot, subdomain, action, null, expectedUrl);

            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: subdomain);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
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
            string expectedStartTag = $"<form action=\"HtmlEncode[[{expectedUrl}]]\" method=\"HtmlEncode[[post]]\">";
            var htmlHelper = ConfigurationFactories.HtmlHelperFactory.Get(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "constantsubdomain",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, null, expectedUrl);

            // Act
            var form = htmlHelper.BeginForm(
                                            actionName: action,
                                            controllerName: controller);

            var writer = Assert.IsAssignableFrom<StringWriter>(htmlHelper.ViewContext.Writer);
            var builder = writer.GetStringBuilder();

            // Assert
            Assert.Equal(expectedStartTag, builder.ToString());
        }
    }
}
